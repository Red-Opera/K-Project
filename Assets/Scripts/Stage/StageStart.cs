using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageStart : MonoBehaviour
{

    public GameObject portal;
    public float interactDistance = 3f; // ��ȣ�ۿ� ������ �Ÿ�

    void Update()
    {
        // �±װ� "Player"�� ������Ʈ�� ���� ������ �Ÿ� ���
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        float distanceToPortal = Vector3.Distance(player.transform.position, portal.transform.position);

        // ���п� ��������鼭 f�� ������ ��
        if (distanceToPortal < interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("�̵��Ҽ��ִ� �Ÿ��Դϴ�.");
            SceneManager.LoadScene("Stage1");
        }
    }
}


