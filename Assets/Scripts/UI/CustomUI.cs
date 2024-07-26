using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CustomUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> players;  // 플레이어 오브젝트
    [SerializeField] private Transform frames;          // 캐릭터 프레임을 담는 오브젝트

    private static int currentPlayerIndex = 2;          // 현재 플레이어 인덱스 값
    private bool isChange = false;                      // 캐릭터를 바꾼 경우

    private void Start()
    {
        Debug.Assert(frames != null, "캐릭터 프레임을 담는 오브젝트가 없습니다.");
    }

    private void Update()
    {
        // 캐릭터를 선택한 경우
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            ChangePlayer();
    }

    // 플레이어를 교체해주는 메소드
    private void ChangePlayer()
    {
        for (int i = 0; i < frames.childCount; i++)
        {
            // 해당 번호의 프레임와 이미지를 가져옴
            Transform frame = frames.GetChild(i).GetChild(0);
            Image frameImage = frame.GetComponent<Image>();

            // 해당 이름의 프레임이 선택된 경우
            if (frameImage.color.a >= 0.98)
            {
                // 이미 선택한 플레이어인 경우
                if (i == currentPlayerIndex)
                    break;

                // 현재 선택한 플레이어 인덱스를 반영하고 플레이어가 바뀜을 알림
                currentPlayerIndex = i;
                isChange = true;

                // 기존 플레이어와 카메라, 위치를 가져옴
                GameObject destroyPlayer = GameObject.FindGameObjectWithTag("Player");
                Camera mainCamera = destroyPlayer.transform.GetChild(0).GetComponent<Camera>();
                Vector2 defaultPlayerPos = new Vector2(destroyPlayer.transform.position.x, destroyPlayer.transform.localPosition.y);

                // 선택한 플레이어를 만들고 카메라를 바꾸고 활성화 후 플레이어 위치를 기존 플레이어 위치로 바꿈
                GameObject newPlayer = Instantiate(players[i]);
                Camera toCamera = newPlayer.transform.GetChild(0).GetComponent<Camera>();
                newPlayer.SetActive(true);
                newPlayer.transform.position = defaultPlayerPos;

                // 카메라 스택을 각각 가져옴
                List<Camera> defualtCameraStack = mainCamera.GetUniversalAdditionalCameraData().cameraStack;
                List<Camera> toCameraStack = toCamera.GetUniversalAdditionalCameraData().cameraStack;

                // 카메라 스택을 복사함
                for (int j = 0; j < defualtCameraStack.Count; j++)
                    toCameraStack.Add(defualtCameraStack[j]);

                newPlayer.GetComponent<PMove>().FindHpBar();
                
                DontDestroyOnLoad(newPlayer);
                Destroy(destroyPlayer);
            }
        }

        // 바뀌었다면 UI를 끔
        if (isChange)
            gameObject.SetActive(false);
    }
}
