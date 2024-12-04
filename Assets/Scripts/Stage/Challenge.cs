using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Challenger : MonoBehaviour
{
    public GameObject[] BossRooms;    // 보스룸 배열
    public MonsterState[] BossStates; 
    public GameObject BossHPBar;      // 보스 HP 바
    private int currentBossRoomIndex; // 현재 보스룸 인덱스
    public Vector3 playerRespawnPosition = new Vector3(12, -4, 0); // 플레이어 위치
    private HpLevelManager hpLevelManager;    
    void Start()
    {
        // 초기 설정
        currentBossRoomIndex = 0;
        hpLevelManager = BossHPBar.GetComponent<HpLevelManager>();
        hpLevelManager.BossState = BossStates[0];

        BossHPBar.SetActive(true);
        // 모든 보스룸 비활성화
        foreach (var bossRoom in BossRooms)
        {
            bossRoom.SetActive(false);
        }

        // 첫 번째 보스룸 활성화
        if (BossRooms.Length > 0)
        {
            BossRooms[currentBossRoomIndex].SetActive(true);
        }

        // 플레이어 초기 위치 설정
        MovePlayerToRespawnPosition();
    }

    void Update()
    {
        CheckBossDefeated();
    }

    void CheckBossDefeated()
    {
        if (currentBossRoomIndex < BossRooms.Length && BossRooms[currentBossRoomIndex] != null)
        {
            // 현재 보스룸에 보스가 존재하는지 확인
            GameObject boss = GameObject.FindWithTag("Boss");
            Debug.Log(boss);
            if (boss == null) // 보스가 죽었으면
            {
                ActivateNextBossRoom();
            }
        }
    }

    void ActivateNextBossRoom()
    {
        if (currentBossRoomIndex < BossRooms.Length - 1)
        {
            // 현재 보스룸 비활성화
            BossRooms[currentBossRoomIndex].SetActive(false);

            // 다음 보스룸 활성화
            currentBossRoomIndex++;
            BossRooms[currentBossRoomIndex].SetActive(true);
            hpLevelManager.BossState = BossStates[currentBossRoomIndex];

            // 플레이어를 새 위치로 이동
            MovePlayerToRespawnPosition();

     
        }
        else
        {
            StartCoroutine(Reset());
        }
    }

    void MovePlayerToRespawnPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerRespawnPosition;
        }
        else
        {
            Debug.LogError("Player를 찾을 수 없습니다. Player 태그가 올바른지 확인하세요.");
        }
    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("2초 후 실행");

        GameObject.Find("EventSystemDonDestory").GetComponent<UINotDestroyOpen>().DestroyThis();
        Destroy(GameObject.FindGameObjectWithTag("Player"));

        SceneManager.LoadScene("Map");
    }
}
