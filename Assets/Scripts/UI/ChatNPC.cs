using System.Collections.Generic;
using UnityEngine;

public class ChatNPC : MonoBehaviour
{
    [SerializeField] private GameObject chat;

    private bool isChat = false;
    private Dialog dialog;

    public void Start()
    {
        dialog = chat.GetComponent<Dialog>();
        Debug.Assert(dialog != null, "대화를 제어할 컴포넌트가 존재하지 않습니다.");
    }

    public void Update()
    {
        
    }

    public void Chat(string name, List<string> chatContent)
    {
        dialog.printList = chatContent;

        chat.SetActive(true);
    }
}
