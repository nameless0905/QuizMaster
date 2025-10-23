using TMPro;
using UnityEngine;

public class endscreen : MonoBehaviour
{
    [SerializeField] scoreKeeper scoreKeeper;
    [SerializeField] TextMeshProUGUI finalscoretext;
    [SerializeField] AudioSource quiz_audio;
    [SerializeField] AudioSource end_audio;

    public void ShowfinalScore()
    {
        finalscoretext.text = "congraturation\r\n" + $"your scored is {scoreKeeper.GetTotalScore()}";
    }

    // endscreen이 활성화될 때 end_audio 재생, quiz_audio 중지
    void OnEnable()
    {
        if (end_audio != null)
        {
            end_audio.Play();
        }

        if (quiz_audio != null)
        {
            quiz_audio.Stop();
        }
    }

    // endscreen이 비활성화될 때 end_audio 중지, quiz_audio 재생
    void OnDisable()
    {
        if (end_audio != null)
        {
            end_audio.Stop();
        }

        if (quiz_audio != null)
        {
            quiz_audio.Play();
        }
    }
}
