using TMPro;
using UnityEngine;

public class quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] questionSO question;
    [SerializeField] TextMeshProUGUI[] answerTexts;
    void Start()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = question.GetAnswer(i);
        }

    }
}
