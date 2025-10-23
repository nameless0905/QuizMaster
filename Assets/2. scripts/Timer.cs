using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] public float problemTime = 5;
    [SerializeField] public float solutionTime = 3f;
    [SerializeField] GameObject TimerImage;
    float Timelimit = 5;
    public float time = 0f;
    string time_Srting;
    float time_copy;
    [HideInInspector] public bool isProblemtime = true;
    [SerializeField] public bool loadNextQuestion;
    [SerializeField] TextMeshProUGUI timerText;

    private void Start()
    {
        time = problemTime;
        Timelimit = problemTime;
        loadNextQuestion = true;
    }
    private void Update()
    {
        time_copy = (float)Math.Round(time, 1);
        time_Srting = time_copy.ToString();
        timerText.text = time_Srting;
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
        TimerImage.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(Color.red, Color.white, time / Timelimit);
    }
    public void CancelTimer()
    {
        time = 0f;
    }
}

