using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static chatGPTclient;

public class chatGPTclient : MonoBehaviour
{
    public delegate void QuizGeneratedHandler(List<questionSO> qustion);
    public event QuizGeneratedHandler QuizGenerateHandler;
    public void GeneratedQuestoins(int questionsCount, string topicToUse)
    {
        Debug.Log($"Generating {questionsCount} questions on the topic: {topicToUse}");
        StartCoroutine(GenerateWithdelay());
    }
    private IEnumerator GenerateWithdelay()
    {
        yield return new WaitForSeconds(2f);
        QuizGenerateHandler?.Invoke(new List<questionSO>());
        Debug.Log("Finished GenrateWith Delay");
    }
}
