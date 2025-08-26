using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] questionSO question;
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] GameObject[] answerButtons;
    void Start()
    {
        GetNextQustion();
    }

    private void GetNextQustion()
    {
        SetButtonState(true);
        OndisplayQuestion();
        SetDefultButtonSprite();
    }


    private void OndisplayQuestion()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.GetAnswer(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
        }
        else
        {
            questionText.text = "wrong!" + question.GetCorrectAnswer();
        }
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
