using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 5f;
    [SerializeField] GameObject TimerImage;
    float Timelimit = 10f;
    float time = 0f;
    [HideInInspector] public bool isProblemtime = true;
    [SerializeField] public bool loadNextQuestion;

    private void Start()
    {
        time = problemTime;
    }
    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            if (isProblemtime)
            {
                isProblemtime = false;
                time = solutionTime; // Switch to solution time
                Timelimit = solutionTime;
            }
            else
            {
                isProblemtime = true;
                time = problemTime; // Switch back to problem time
                Timelimit = problemTime;
                loadNextQuestion = true;
            }
        }
        TimerImage.GetComponent<UnityEngine.UI.Image>().fillAmount = time / Timelimit;
    }
    public void CancelTimer()
    {
        time = 0f;
    }
}

