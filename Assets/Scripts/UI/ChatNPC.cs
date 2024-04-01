using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatNPC : MonoBehaviour
{
    [HideInInspector] public Dialog dialog;             // ��ȭ ��ũ��Ʈ

    [SerializeField] private GameObject chat;           // ��ȭ�ϴµ� �ʿ��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI nameText;  // �̸��� ǥ���ϴ��� �ؽ�Ʈ
    [SerializeField] private List<string> chatContent;  // ä���� ����

    public void Start()
    {
        dialog = chat.GetComponent<Dialog>();
        Debug.Assert(dialog != null, "��ȭ�� ������ ������Ʈ�� �������� �ʽ��ϴ�.");
    }

    public void Chat(string name)
    {
        dialog.printList = chatContent;
        nameText.text = name;

        chat.SetActive(true);
    }
}
