using UnityEngine;

public class scoreKeeper : MonoBehaviour
{
    int correctAnswer = 0;
    int QuestionsSeen = 0;
    int totalScore = 0;

    public int GetCorrectAnswer() => correctAnswer;
    public int GetQuestionSeen() => QuestionsSeen;
    public int GetTotalScore() => totalScore;

    public void IncrementCorrectAnswer() => correctAnswer++;
    public void IncrementQuestionSeen() => QuestionsSeen++;
    public void AddScore(int score) => totalScore += score;
}
