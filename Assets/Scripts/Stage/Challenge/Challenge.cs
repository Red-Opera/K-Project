using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Challenge : MonoBehaviour
{
    public GameObject lobbyRoom;             // 로비 오브젝트
    public GameObject[] bossRooms;           // 보스룸 배열
    public GameObject[] bossObjects;         // 보스 오브젝트 배열
    public GameObject triggerObject;
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private int currentBossRoomIndex = -1;   // 현재 보스룸의 인덱스
    private GameObject player;               // 플레이어 오브젝트

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player 태그가 붙은 오브젝트가 존재하지 않습니다.");
        }

        lobbyRoom.SetActive(true);
        foreach (GameObject bossRoom in bossRooms)
        {
            bossRoom.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == triggerObject && other.CompareTag("Player") && currentBossRoomIndex == -1)
        {
            StartCoroutine(TransitionToBossRoom(0)); // 첫 번째 보스룸으로 이동
        }
    }

    private IEnumerator TransitionToBossRoom(int bossRoomIndex)
    {
        float elapsedTime = 0;
        Color fadeColor = fadeImage.color;
        fadeColor.a = 0;
        fadeImage.color = fadeColor; // 초기 알파값 설정

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        lobbyRoom.SetActive(false);
        if (currentBossRoomIndex >= 0)
        {
            bossRooms[currentBossRoomIndex].SetActive(false);
        }

        currentBossRoomIndex = bossRoomIndex;
        bossRooms[currentBossRoomIndex].SetActive(true);

        Transform bossRoomStartPoint = bossRooms[currentBossRoomIndex].transform.Find("StartPoint");
        if (bossRoomStartPoint != null)
        {
            player.transform.position = bossRoomStartPoint.position;
        }
        else
        {
            Debug.LogWarning("StartPoint가 현재 보스룸에 없습니다.");
        }

        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }
    }

    public void GoToNextBossRoom()
    {
        if (currentBossRoomIndex < bossRooms.Length - 1)
        {
            // 모든 보스 오브젝트가 파괴되었는지 확인
            bool allBossesDestroyed = true;
            foreach (GameObject boss in bossObjects)
            {
                if (boss != null) // 보스 오브젝트가 파괴되지 않았다면 false 설정
                {
                    allBossesDestroyed = false;
                    break;
                }
            }

            // 모든 보스 오브젝트가 파괴되었을 경우에만 다음 보스룸으로 이동
            if (allBossesDestroyed)
            {
                StartCoroutine(TransitionToBossRoom(currentBossRoomIndex + 1));
            }
            else
            {
                Debug.Log("아직 모든 보스를 클리어하지 않았습니다.");
            }
        }
        else
        {
            Debug.Log("모든 보스를 클리어했습니다!");
        }
    }
}
