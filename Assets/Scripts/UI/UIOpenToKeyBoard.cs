using System.Security.Cryptography;
using UnityEngine;

public class UIOpenToKeyBoard : MonoBehaviour
{
    [SerializeField] private GameObject pressedKeyImage;    // 올라가는 이미지 오브젝트
    [SerializeField] private AnimationCurve speedCurve;     // 시간에 따른 추가로 이동한 위치를 나타내는 AnimationCurve
    [SerializeField] private Vector3 originalPosition;      // 이미지의 원래 위치
    [SerializeField] private GameObject openUI;             // UI 킬 대상

    private bool isRising = false;  // 올라가는 중인지 여부
    private bool isFalling = false; // 내려가는 중인지 여부
    private bool isEnter = false;   // 플레이어가 들어왔는지 여부
    private float targetHeight;     // 올라가는 목표 높이
    private float startTime;        // 동작 시작 시간
    private float reverseTime;      // 뒤로 가는 시간

    public void Start()
    {
        Debug.Assert(pressedKeyImage != null, "눌러야 하는 키 UI가 없습니다.");
        originalPosition = pressedKeyImage.transform.position;              // 원래 위치 저장
        pressedKeyImage.SetActive(false);                                   // 시작 시 이미지 비활성화
    }

    public void Update()
    {
        // 올라가는 중이면
        if (isRising)
        {
            if (reverseTime < 0)
                reverseTime = 0.0f;

            // 현재까지 진행된 시간에 대한 위치 값 계산
            float timeSinceStart = Time.time - startTime + reverseTime;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // 이미지를 천천히 올림
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // 목표 높이에 도달하면 올라가는 동작 종료
            if (pressedKeyImage.transform.position.y >= targetHeight)
                isRising = false;
        }

        // 내려가는 중이면
        if (isFalling)
        {
            // 현재까지 진행된 시간에 대한 위치 값 계산
            float timeSinceStart = reverseTime + startTime - Time.time;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // 이미지를 천천히 내림
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // 원래 위치에 도달하면 내려가는 동작 종료
            if (pressedKeyImage.transform.position.y <= targetHeight)
            {
                isFalling = false;
                pressedKeyImage.transform.position = originalPosition; // 원래 위치로 이동
            }
        }

        if (isEnter && !openUI.activeSelf && Input.GetKeyDown(KeyCode.P))
            openUI.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 충돌한 오브젝트가 플레이어인 경우 이미지를 활성화하고 올라가기 시작
            pressedKeyImage.SetActive(true);

            isRising = true;
            isFalling = false;  // 내려가는 동작이 실행 중이 아니도록 설정
            isEnter = true;     // 플레이어가 들어왔음을 표시

            targetHeight = pressedKeyImage.transform.position.y + speedCurve.keys[speedCurve.length - 1].value;   // 목표 높이 설정
            reverseTime = reverseTime + startTime - Time.time;          // 동작 시작 시간 저장
            startTime = Time.time;                                      // 동작 시작 시간 저장
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 트리거 영역을 빠져나갔을 때는 내려가는 동작 시작
            isRising = false;
            isFalling = true;
            isEnter = false;

            targetHeight = originalPosition.y;      // 목표 높이를 원래 위치로 설정
            reverseTime = Time.time - startTime;    // 동작 시작 시간 저장
            startTime = Time.time;
        }
    }
}