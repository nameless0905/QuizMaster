using UnityEngine;

public class scoreKeeper : MonoBehaviour
{
    int correctAnswer = 0;
    int QuestionsSeen = 0;

    public int GetCorrectAnswer()
    {
        return correctAnswer;
    }
    public int GetQuestionSeen()
    {
        return QuestionsSeen;
    }
    public void IncrementCorrectAnswer()
    {
        correctAnswer++;
    }
    public void IncrementQuestionSeen()
    {
        QuestionsSeen++;
    }
    public int calculateScore()
    {
        return Mathf.RoundToInt((float)correctAnswer / (float)QuestionsSeen * 100);
    }
}
