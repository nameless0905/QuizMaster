using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static chatGPTclient;

public class chatGPTclient : MonoBehaviour
{
    public delegate void QuizGeneratedHandler(List<questionSO> question);
    public event QuizGeneratedHandler QuizGenerateHandler;
    public void GeneratedQuestoins(int questionsCount, string topicToUse)
    {
        Debug.Log($"Generating {questionsCount} questions on the topic: {topicToUse}");
        StartCoroutine(GenerateWithdelay());
    }
    private IEnumerator GenerateWithdelay()
    {
        yield return new WaitForSeconds(3f);
        QuizGenerateHandler?.Invoke(new List<questionSO>());
        List<questionSO> questions = new List<questionSO>();
        questionSO so1 = CreateQuestion("GPT생성질문1", 
            new string[]{"답변1(정답)", "답변2", "답변3", "답변4"},
             0);
        questions.Add(so1);
        questionSO so2 = CreateQuestion("GPT생성질문2",
            new string[] { "답변1(정답)", "답변2", "답변3", "답변4" },
             1);
        questions.Add(so1);
        questionSO so3 = CreateQuestion("GPT생성질문3",
            new string[] { "답변1(정답)", "답변2", "답변3", "답변4" },
             2);
        questions.Add(so1);

        QuizGenerateHandler?.Invoke(questions);
        Debug.Log("Finished GenrateWith Delay");
    }

questionSO CreateQuestion(string q, string[] a, int correctIndex)
    {
        questionSO so = ScriptableObject.CreateInstance<questionSO>();
        so.SetData(q, a, correctIndex);
        
        return so;
    }
}
