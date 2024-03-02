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
        // ������Ʈ�� Ȱ��ȭ�� ������ ��ġ�� (0,0,0)���� ����
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

        // Town ������ ���ư���
        SceneManager.LoadScene("Map");
    }
}

