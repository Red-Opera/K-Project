using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageStart : MonoBehaviour
{

    public GameObject portal;
    public float interactDistance = 3f; // 상호작용 가능한 거리

    void Update()
    {
        // 태그가 "Player"인 오브젝트와 포털 사이의 거리 계산
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distanceToPortal = Vector3.Distance(player.transform.position, portal.transform.position);

        // 포털에 가까워지면서 f를 눌렀을 때
        if (distanceToPortal < interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("이동할수있는 거리입니다.");
            SceneManager.LoadScene("Stage1");
        }
    }
}


