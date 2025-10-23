using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static ChatGPTClient;

public class quiz : MonoBehaviour
{
    [Header("Question")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI hint;
    questionSO currentquestion;

    [Header("Button Colors")]
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite defaultAnswerSprite;

    [Header("Choices")]
    [SerializeField] GameObject[] answerButtons;

    [Header("Timer")]
    [SerializeField] Image TimerImage;
    [SerializeField] Sprite solutionTimerSprite;
    [SerializeField] Sprite problemTimerSprite;
    Timer timer;
    bool chooseAnswer = false;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    scoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider slider;
    public bool isComplete;

    [Header("ChatGPT")]
    [SerializeField] ChatGPTClient chatGPTclient;
    [SerializeField] int questionsCount = 10; // use 10 questions
    [SerializeField] TextMeshProUGUI loadingtext;
    bool isGenerating = false;

    bool isGenerateQuestion = false;
    public string topicToUse = GameManeger.instance.topic_test; // may be any runtime string
    List<questionSO> generatedQuestions = new List<questionSO>();
    int currentQuestionIndex = 0;

    // FIX: block the very first loadNextQuestion once right after generation
    bool blockNextQuestionOnce = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<scoreKeeper>();
        chatGPTclient.quizGenerateHandler += QuizGeneratedHandler;
        GenerateionsifNeeded();
    }

    private void OnDestroy()
    {
        if (chatGPTclient != null)
            chatGPTclient.quizGenerateHandler -= QuizGeneratedHandler;
    }

    public void GenerateionsifNeeded()
    {
        if (isGenerateQuestion) return;

        isGenerateQuestion = true;
        GameManeger.instance.ShowLoadingSceen();

        string topic = string.IsNullOrEmpty(topicToUse) ? "Science" : topicToUse;
        chatGPTclient.GenerateQuizQuestions(questionsCount, topic);
    }

    private string GetTrendingtopic()
    {
        string[] topics = { "Science", "History", "Music", "Movie", "Sports", "Technology", "Literature", "Art", "Geography", "Politics" };
        int randomindex = UnityEngine.Random.Range(0, topics.Length);
        return topics[randomindex];
    }

    void QuizGeneratedHandler(List<questionSO> questionsFromGPT)
    {
        Debug.Log($"quizGeneratedHandler : {questionsFromGPT?.Count ?? 0} questions received");
        isGenerateQuestion = false;

        if (questionsFromGPT == null || questionsFromGPT.Count == 0)
        {
            Debug.LogError("No questions generated. Please try again.");
            loadingtext.text = "Question Loading Failed";
            return;
        }

        // FIX A: reset timer/status before showing the first question
        if (timer != null)
        {
            timer.loadNextQuestion = false;   // prevent immediate skip to next
            timer.isProblemtime = true;       // ensure we are in problem phase
        }
        chooseAnswer = false;
        blockNextQuestionOnce = true;         // suppress one stray loadNextQuestion tick

        generatedQuestions = questionsFromGPT;
        slider.maxValue = generatedQuestions.Count;
        slider.value = 0;
        currentQuestionIndex = 0;

        ShowCurrentQuestion();
        GameManeger.instance.ShowQuizSceen();
    }

    private void Update()
    {
        if (timer.time < timer.problemTime/2 && !chooseAnswer && currentquestion != null)
        {
            hint.text = "Hint: " + currentquestion.GetHint();
        }
        if (timer.isProblemtime)
        {
            TimerImage.sprite = problemTimerSprite;
        }
        else
        {
            TimerImage.sprite = solutionTimerSprite;
        }

        // FIX B: on the very first frame after generation, clear any stale flag
        if (blockNextQuestionOnce)
        {
            blockNextQuestionOnce = false;
            if (timer != null) timer.loadNextQuestion = false;
        }

        // Move to next question when timer requests it
        if (timer.loadNextQuestion)
        {
            GoToNextQuestion();
        }

        // Auto-solution runs only once when time is over
        if (!timer.isProblemtime && !chooseAnswer && currentquestion != null)
        {
            DisplaySolution(-1);
            chooseAnswer = true;
        }
    }

    private void NextQuestion()
    {
        timer.loadNextQuestion = false;
        GameManeger.instance.ShowQuizSceen();
        chooseAnswer = false;
        SetButtonState(true);
        SetDefultButtonSprite();
        ShowCurrentQuestion();
        scoreKeeper.IncrementQuestionSeen();
        slider.value++;
        hint.text = "";
    }

    private void ShowCurrentQuestion()
    {
        if (currentQuestionIndex >= generatedQuestions.Count)
        {
            Debug.Log("No more questions");
            return;
        }

        currentquestion = generatedQuestions[currentQuestionIndex];

        Debug.Log(currentquestion.GetQuestion());
        questionText.text = currentquestion.GetQuestion();
        

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentquestion.GetAnswer(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        if (currentquestion == null) return;

        chooseAnswer = true;

        // Score only if correct (before CancelTimer)
        if (index == currentquestion.GetCorrectAnswerIndex())
        {
            float ratio = timer.time / timer.problemTime; // remaining time ratio
            int score = 0;
            if (ratio >= 0.8f)
                score = 100;
            else if (ratio >= 0.5f)
                score = 70;
            else
                score = 50;

            scoreKeeper.AddScore(score);
        }

        DisplaySolution(index);
        timer.CancelTimer();

        scoreText.text = "Score: " + scoreKeeper.GetTotalScore();
    }

    private void DisplaySolution(int index)
    {
        if (currentquestion == null) return;

        if (index == currentquestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            if (index >= 0 && index < answerButtons.Length)
            {
                answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            }
            scoreKeeper.IncrementCorrectAnswer();
        }
        else
        {
            questionText.text = "Wrong! " + currentquestion.GetCorrectAnswer();
            // If you also want to highlight the correct button:
            // int correct = currentquestion.GetCorrectAnswerIndex();
            // if (correct >= 0 && correct < answerButtons.Length)
            //     answerButtons[correct].GetComponent<Image>().sprite = correctAnswerSprite;
        }
        SetButtonState(false);
    }

    private void SetDefultButtonSprite()
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }

    private void SetButtonState(bool state)
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Button>().interactable = state;
        }
    }

    // Move to next question
    public void GoToNextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < generatedQuestions.Count)
        {
            NextQuestion();
        }
        else
        {
            // All questions finished
            timer.loadNextQuestion = false; // prevent duplicate triggers
            GameManeger.instance.ShowendSceen();

            // If you prefer auto-regeneration, call:
            // GenerateionsifNeeded();
        }
    }
}

