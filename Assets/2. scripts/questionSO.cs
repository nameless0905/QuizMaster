using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class questionSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField]string question = "여기에 질문을 입력하세요.";

}
