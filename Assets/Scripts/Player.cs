using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �÷��̾� GameObject�� �ı����� ����
            transform.position = Vector3.zero;
        }
        else
        {
            Destroy(gameObject); // �̹� ���� �ִ� ��� �ߺ��� �÷��̾� GameObject �ı�
        }
    }
}
