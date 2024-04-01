using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI contentText;     // ��ȭ�� ����ϱ� ���� �ؽ�Ʈ
    public List<string> printList;          // ��ȭ�� ����ؾ� �� ���

    public float typingDelay = 0.1f;

    [HideInInspector] public bool isChat = false;

    private int currentLineIndex = 0;       // ���� ��� ���� ��ȭ�� �ε���
    private bool isPrinting = false;        // ��ȭ ��� ������ ����

    void Start()
    {
        Debug.Assert(contentText != null, "������ ǥ���� �ؽ�Ʈ�� �������� �ʽ��ϴ�.");
    }

    public void OnEnable()
    {
        currentLineIndex = 0;
        isChat = true;

        StartCoroutine(PrintLine(printList[currentLineIndex]));
    }

    void Update()
    {
        // �Է� Ȯ�� (���콺 Ŭ�� �Ǵ� �����̽� ��)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // �� ����� ��ȭ�� �ִ��� Ȯ��
            if (currentLineIndex < printList.Count)
            {
                // ��� ���� �ƴ� ��� ���� �� ��� ����
                if (!isPrinting)
                    StartCoroutine(PrintLine(printList[currentLineIndex]));

                else
                {
                    StopAllCoroutines();                                // ������ ��� �ڷ�ƾ ����
                    contentText.text = printList[currentLineIndex];     // ���� �� ��� ���
                    currentLineIndex++;                                 // ���� �ٷ� �̵�

                    isPrinting = false;
                }
            }

            // ��� ��ȭ�� ��µ�, �ʿ信 ���� ���⿡ �߰� ������ �߰��� �� ����
            else
            {
                isChat = false;
                gameObject.SetActive(false);
            }
        }
    }

    // ��ȭ�� ������ �Բ� ����ϴ� �ڷ�ƾ
    private IEnumerator PrintLine(string line)
    {
        isPrinting = true;
        contentText.text = ""; // ���� �ؽ�Ʈ �����

        foreach (char letter in line)
        {
            contentText.text += letter;
            yield return new WaitForSeconds(typingDelay);
        }

        // ���� �ٷ� �̵�
        currentLineIndex++;

        isPrinting = false;

        // ����� �Է��� ��ٸ��� �ʰ� �ڵ����� ���� �ٷ� �̵��Ϸ��� �Ʒ� �ּ� ����
        // UpdateContent();
    }
}
