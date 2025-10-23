using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    [SerializeField] private quiz quiz;
    [SerializeField] private endscreen endscreen;
    [SerializeField] private GameObject loadingCanvas;
    public string topic_test;
    public static GameManeger instance { get; private set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //ShowQuizScreen();
    }

    public void ShowLoadingSceen()
    {
        quiz.gameObject.SetActive(false);
        endscreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(true);
    }
    public void ShowQuizSceen()
    {
        quiz.gameObject.SetActive(true);
        endscreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }
    public void ShowendSceen()
    {
        quiz.gameObject.SetActive(false);
        endscreen.gameObject.SetActive(true);
        endscreen.ShowfinalScore();
        loadingCanvas.SetActive(false);
    }
    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Next_scean()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("gameScene");
    }

    public void GenerateScienceQuiz(int count = 3)
    {
        topic_test = "과학";
        Next_scean();
    }

    public void GenerateHumorQuiz(int count = 3)
    {
        topic_test = "유머";
        Next_scean();
    }

    public void GenerateGeneralQuiz(int count = 3)
    {
        topic_test = "상식";
        Next_scean();
    }

    public void GenerateNonsenseQuiz(int count = 3)
    {
        topic_test = "넌센스";
        Next_scean();
    }
}
