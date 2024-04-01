using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatNPC : MonoBehaviour
{
    [HideInInspector] public Dialog dialog;             // 대화 스크립트

    [SerializeField] private GameObject chat;           // 대화하는데 필요한 텍스트
    [SerializeField] private TextMeshProUGUI nameText;  // 이름을 표시하느느 텍스트
    [SerializeField] private List<string> chatContent;  // 채팅할 내용

    public void Start()
    {
        dialog = chat.GetComponent<Dialog>();
        Debug.Assert(dialog != null, "대화를 제어할 컴포넌트가 존재하지 않습니다.");
    }

    public void Chat(string name)
    {
        dialog.printList = chatContent;
        nameText.text = name;

        chat.SetActive(true);
    }
}
