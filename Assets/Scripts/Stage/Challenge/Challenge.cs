using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Challenge : MonoBehaviour
{
    public GameObject lobbyRoom;       // 로비 오브젝트
    public GameObject[] bossRooms;     // 보스룸 배열
    public Image fadeImage;            
    public float fadeDuration = 1.0f; 

    private GameObject player;         // player 태그의 오브젝트를 자동으로 찾기
    private int currentBossRoomIndex = -1; // 현재 보스룸의 인덱스

    void Start()
    {
        // player 태그가 붙은 오브젝트 찾기
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 태그가 붙은 오브젝트가 존재하지 않습니다.");
        }

        // 로비를 활성화하고 모든 보스룸을 비활성화
        lobbyRoom.SetActive(true);
        foreach (GameObject bossRoom in bossRooms)
        {
            bossRoom.SetActive(false);
        }
    }

    // 로비의 특정 오브젝트와 충돌 시 보스룸으로 이동
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentBossRoomIndex == -1)
        {
            StartCoroutine(TransitionToBossRoom(0)); // 첫 번째 보스룸으로 이동
        }
    }

    private IEnumerator TransitionToBossRoom(int bossRoomIndex)
    {
        // 페이드아웃
        float elapsedTime = 0;
        Color fadeColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        // 로비 비활성화 및 보스룸 활성화
        lobbyRoom.SetActive(false);
        if (currentBossRoomIndex >= 0)
        {
            bossRooms[currentBossRoomIndex].SetActive(false); // 이전 보스룸 비활성화
        }

        // 새로운 보스룸 활성화 및 플레이어 위치 이동
        currentBossRoomIndex = bossRoomIndex;
        bossRooms[currentBossRoomIndex].SetActive(true);

        if (player != null)
        {
            player.transform.position = new Vector3(0, 0, 0); // 새로운 보스룸 내 위치 설정
        }

        // 페이드인
        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }
    }

    // 다음 보스룸으로 이동하는 메서드
    public void GoToNextBossRoom()
    {
        if (currentBossRoomIndex < bossRooms.Length - 1)
        {
            StartCoroutine(TransitionToBossRoom(currentBossRoomIndex + 1));
        }
        else
        {
            Debug.Log("모든 보스를 클리어했습니다!");
        }
    }
}
