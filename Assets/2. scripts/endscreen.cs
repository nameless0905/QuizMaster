using TMPro;
using UnityEngine;

public class endscreen : MonoBehaviour
{
    [SerializeField] scoreKeeper scoreKeeper;
    [SerializeField] TextMeshProUGUI finalscoretext;
    public void ShowfinalScore()
    {
        finalscoretext.text = "congraturation\r\n"+$"your scored is {scoreKeeper.calculateScore()}%";
    }

}
