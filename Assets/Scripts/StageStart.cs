using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageStart : MonoBehaviour
{
 
    public float interactDistance = 3f; // ��ȣ�ۿ� ������ �Ÿ�
    void OnTriggerStay(Collider other)
    {
        // �÷��̾ ���� �Ÿ� ���� �ְ� �����̽��ٸ� ������ ��
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
        {
            // ���� ������ �Ѿ
            SceneManager.LoadScene("Stage1");
        }
    }
}
