using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static chatGPTclient;

public class quiz : MonoBehaviour
{
    [Header("질문")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<questionSO> questions = new List<questionSO>();
    questionSO currentquestion;

    [Header("버튼 색")]
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite defaultAnswerSprite;

    [Header("보기")]
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

    [Header("prograssbar")]
    [SerializeField] Slider slider;
    public bool isComplete;

    [Header("ChatGPT")]
    [SerializeField] chatGPTclient chatGPTclient;
    [SerializeField] int questionsCount = 3;
    bool isGenerating = false;

    bool isGenerateQuestion = false;

    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<scoreKeeper>();
        chatGPTclient.QuizGenerateHandler += QuizGeneratedHandler;

        if (questions.Count == 0)
        {
            GenerateionsifNeeded();
        }
        else
        {
            initiallizeslider();
        }
    }

    private void GenerateionsifNeeded()
    {
        if (isGenerateQuestion) return;
        
        isGenerateQuestion = true;
        GameManeger.instance.ShowLoadingSceen();

        string topicToUse = GetTrendingtopic();
        chatGPTclient.GeneratedQuestoins(questionsCount, topicToUse);
    }

    private string GetTrendingtopic()
    {
        string[] topics = { "과학", "역사", "음악", "영화", "스포츠", "기술", "문학", "예술", "지리", "정치" };
        int randomindex = UnityEngine.Random.Range(0, topics.Length);
        return topics[randomindex];
    }

    void QuizGeneratedHandler(List<questionSO> questions)
    {
        Debug.Log($"quizGeneratedHandler : {questions.Count} questions received");
        isGenerateQuestion = false;
    }
    private void initiallizeslider()
    {
        slider.maxValue = questions.Count;
        slider.value = 0;

    }

    private void Update()
    {
        if (timer.isProblemtime)
        {
            TimerImage.sprite = problemTimerSprite;
        }
        else
        {
            TimerImage.sprite = solutionTimerSprite;
        }


        if (timer.loadNextQuestion)
        {
            if (questions.Count <= 0)
            {
                GenerateionsifNeeded();
                //GameManeger.instance.ShowendSceen();
            }
            else
            {
                timer.loadNextQuestion = false;
                GetNextQustion();
            }
        }

        if (!timer.isProblemtime && !chooseAnswer)
        {
            DisplaySolution(-1);
        }
    }
    private void GetNextQustion()
    {
        if (questions.Count <= 0)
        {
            Debug.Log("No more questions");
            return;
        }
        GameManeger.instance.ShowQuizSceen();
        chooseAnswer = false;
        SetButtonState(true);
        SetDefultButtonSprite();
        GetRendomQuestion();
        OndisplayQuestion();
        scoreKeeper.IncrementQuestionSeen();
        slider.value++;
    }

    private void GetRendomQuestion()
    {
        int Randomindex = UnityEngine.Random.Range(0, questions.Count);
        currentquestion = questions[Randomindex];
        questions.RemoveAt(Randomindex);
    }

    private void OndisplayQuestion()
    {
        Debug.Log(currentquestion.GetQuestion());
        questionText.text = currentquestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentquestion.GetAnswer(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        chooseAnswer = true;
        DisplaySolution(index);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.calculateScore() + "%";

        
    }

    private void DisplaySolution(int index)
    {
        if (index == currentquestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswer();
        }
        else
        {
            questionText.text = "wrong!" + currentquestion.GetCorrectAnswer();
        }
        //timer.CancelTimer();
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
}
