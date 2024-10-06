using System.Collections;
using UnityEngine;

public class UIOpenToKeyBoard : MonoBehaviour
{
    [SerializeField] private GameObject pressedKeyImage;    // 키가 눌렸을 때 표시되는 이미지
    [SerializeField] private AnimationCurve speedCurve;     // 이미지가 움직이는 속도를 제어하는 AnimationCurve
    [SerializeField] private Vector3 originalPosition;      // 이미지의 원래 위치
    [SerializeField] private GameObject openUI;             // 열릴 UI 창

    private ChatNPC chatNPC;        // NPC와 대화하는 기능을 담은 스크립트 참조

    private bool isRising = false;  // 이미지가 상승 중인지 여부
    private bool isFalling = false; // 이미지가 하강 중인지 여부
    private bool isEnter = false;   // 플레이어가 트리거 영역에 있는지 여부
    private float targetHeight;     // 이미지가 도달할 목표 높이
    private float startTime;        // 애니메이션 시작 시간
    private float reverseTime;      // 역방향 애니메이션 시간
    private PMove pMove;
    private WeaponController weaponController;

    public void Start()
    {
        Debug.Assert(pressedKeyImage != null, "키 이미지가 설정되지 않았습니다.");
        originalPosition = pressedKeyImage.transform.position;              // 이미지의 원래 위치 저장
        pressedKeyImage.SetActive(false);                                   // 처음에는 이미지가 보이지 않도록 설정

        chatNPC = GetComponent<ChatNPC>();
    }

    public void Update()
    {
        // 이미지가 상승 중일 때
        if (isRising)
        {
            pressedKeyImage.SetActive(true);

            if (reverseTime < 0)
                reverseTime = 0.0f;

            // 애니메이션이 진행된 시간에 따른 새로운 높이 계산
            float timeSinceStart = Time.time - startTime + reverseTime;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // 이미지의 새로운 위치 설정
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // 목표 높이에 도달하면 상승 상태 종료
            if (pressedKeyImage.transform.position.y >= targetHeight)
                isRising = false;
        }

        // 이미지가 하강 중일 때
        if (isFalling)
        {
            // 애니메이션이 진행된 시간에 따른 새로운 높이 계산
            float timeSinceStart = reverseTime + startTime - Time.time;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // 이미지의 새로운 위치 설정
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // 원래 위치에 도달하면 하강 상태 종료
            if (pressedKeyImage.transform.position.y <= targetHeight)
            {
                isFalling = false;
                pressedKeyImage.SetActive(false);
                pressedKeyImage.transform.position = originalPosition; // 원래 위치로 복귀
            }
        }

        // 플레이어가 트리거 영역에 있고, UI가 열리지 않았으며, F키를 눌렀을 때
        if (isEnter && !openUI.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            if (chatNPC != null && !chatNPC.dialog.isChat)
            {
                chatNPC.Chat("BlackSmith");
                pMove.SendDialog(chatNPC.dialog);
                weaponController.SendDialog(chatNPC.dialog);
                StartCoroutine(WaitChat());
            }
            else
            {
                openUI.SetActive(true);
                pMove.SendUI(openUI);
                weaponController.SendUI(openUI);
                Debug.Log(openUI.tag);
            }
        }
    }

    private IEnumerator WaitChat()
    {
        while (chatNPC.dialog.isChat)
            yield return null;

        openUI.SetActive(true);
        pMove.SendUI(openUI);
        weaponController.SendUI(openUI);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pMove = other.GetComponent<PMove>();
            weaponController = other.GetComponent<WeaponController>();
            // 플레이어가 트리거에 진입할 때 상승 시작
            pressedKeyImage.SetActive(true);

            isRising = true;
            isFalling = false;  // 하강 중이 아니라는 상태로 변경
            isEnter = true;     // 플레이어가 영역에 있음을 표시

            targetHeight = pressedKeyImage.transform.position.y + speedCurve.keys[speedCurve.length - 1].value;   // 목표 높이 설정
            reverseTime = reverseTime + startTime - Time.time;          // 애니메이션 시작 시간을 계산
            startTime = Time.time;                        // 현재 시간을 시작 시간으로 설정
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 영역에서 벗어나면 하강 시작
            isRising = false;
            isFalling = true;
            isEnter = false;

            targetHeight = originalPosition.y;      // 목표 높이를 원래 위치로 설정
            reverseTime = Time.time - startTime;    // 애니메이션 시간 계산
            startTime = Time.time;
        }
    }
}
