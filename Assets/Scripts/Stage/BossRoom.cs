using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject tilemapToActivate;

    //특정 구역 넘어 갔을 때 보스방 입구를 막는 벽 생성
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemapToActivate.gameObject.SetActive(true);
        }
    }
}
