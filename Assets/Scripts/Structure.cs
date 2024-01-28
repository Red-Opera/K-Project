using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int recoveryAmount = 50;
    public float interactionRange = 2f;

    private bool isUsed = false;
     void Update()
    {
        // �÷��̾�� ������ ������ �Ÿ��� ���
        float distance = Vector2.Distance(transform.position, PlayerController.instance.transform.position);

        // �÷��̾ ���������� ��ȣ�ۿ� �Ÿ� ���� �ְ�, �����̽��ٸ� ��������, �������� ���� ������ ���� ������ ���
        if (distance <= interactionRange && Input.GetKeyDown(KeyCode.Space) && !isUsed)
        {
            // �÷��̾��� ü���� ȸ��
            PlayerHealth playerHealth = PlayerController.instance.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(recoveryAmount); // �÷��̾��� ȸ�� �Լ� ȣ��
                isUsed = true; // �������� ����� ���·� ����
                gameObject.SetActive(false); // ������ ��Ȱ��ȭ
            }
        }
    }
}