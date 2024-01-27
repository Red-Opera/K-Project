using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI contentText;     // 대화를 출력하기 위한 텍스트
    public List<string> printList;          // 대화를 출력해야 할 목록

    public float typingDelay = 0.1f;

    private int currentLineIndex = 0;       // 현재 출력 중인 대화의 인덱스
    private bool isPrinting = false;        // 대화 출력 중인지 여부

    void Start()
    {
        Debug.Assert(contentText != null, "내용을 표시할 텍스트가 존재하지 않습니다.");
        StartCoroutine(PrintLine(printList[currentLineIndex]));
    }

    void Update()
    {
        // 입력 확인 (마우스 클릭 또는 스페이스 바)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // 더 출력할 대화가 있는지 확인
            if (currentLineIndex < printList.Count)
            {
                // 출력 중이 아닌 경우 다음 줄 출력 시작
                if (!isPrinting)
                    StartCoroutine(PrintLine(printList[currentLineIndex]));

                else
                {
                    StopAllCoroutines();                                // 기존의 출력 코루틴 중지
                    contentText.text = printList[currentLineIndex];     // 현재 줄 즉시 출력
                    currentLineIndex++;                                 // 다음 줄로 이동

                    isPrinting = false;
                }
            }

            // 모든 대화가 출력됨, 필요에 따라 여기에 추가 로직을 추가할 수 있음
            else
                gameObject.SetActive(false);
        }
    }

    // 대화를 지연과 함께 출력하는 코루틴
    IEnumerator PrintLine(string line)
    {
        isPrinting = true;
        contentText.text = ""; // 기존 텍스트 지우기

        foreach (char letter in line)
        {
            contentText.text += letter;
            yield return new WaitForSeconds(typingDelay);
        }

        // 다음 줄로 이동
        currentLineIndex++;

        isPrinting = false;

        // 사용자 입력을 기다리지 않고 자동으로 다음 줄로 이동하려면 아래 주석 해제
        // UpdateContent();
    }

    // 현재 줄로 콘텐츠 텍스트 업데이트
    void UpdateContent()
    {
        if (currentLineIndex < printList.Count)
            contentText.text = printList[currentLineIndex];
    }
}
