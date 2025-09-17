using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class questionSO : ScriptableObject
{
    [TextArea(2,6)  ]
    [SerializeField] string question = "여기에 질문을 입력하세요.";
    [SerializeField] string hint = "여기에 힌트를 입력하세요.";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex = 0;
    public string GetQuestion()
    {
        return question;
    }
    public string GetAnswer(int index)
    {
        return answers[index];
    }
    public string GetCorrectAnswer()
    {
        return answers[correctAnswerIndex];
    }
    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }
    public void SetData(string q, string[] a, int correctIndex, string h)
    {
        SetData(q, a, correctIndex);
        hint = h;
    }
    public void SetData(string q, string[] a, int correctIndex)
    {
        question = q;
        answers = a;
        correctAnswerIndex = correctIndex;
    }

    internal string GetHint()
    {
        return hint;
    }
}
