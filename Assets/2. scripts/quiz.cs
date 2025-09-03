using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<scoreKeeper>();
        slider.maxValue = questions.Count;
        slider.value = 0;

        GetNextQustion();
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
            timer.loadNextQuestion = false;
            GetNextQustion();
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
