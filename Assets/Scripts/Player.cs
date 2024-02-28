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
            DontDestroyOnLoad(gameObject); // 플레이어 GameObject를 파괴하지 않음
            transform.position = Vector3.zero;
        }
        else
        {
            Destroy(gameObject); // 이미 씬에 있는 경우 중복된 플레이어 GameObject 파괴
        }
    }
}
