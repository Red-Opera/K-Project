using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int stageIndex;
    public GameObject[] Stages;
    public GameObject BossStage;
    public GameObject player;

    void Start()
    {
        // 오브젝트가 활성화될 때마다 위치를 (0,0,0)으로 설정
        transform.position = Vector3.zero;
    }
    public void NextStage()
    {
        //Change Stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
        }
        else
        {
            Stages[stageIndex].SetActive(false);
            BossStage.SetActive(true); 
        }

        
    }
    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.GetComponent<PlayerMove>().VelocityZero();
    }
    public void PlayerDied()
    {
        Debug.Log("Player died. Returning to Town.");

        // Town 씬으로 돌아가기
        SceneManager.LoadScene("Map");
    }
}

