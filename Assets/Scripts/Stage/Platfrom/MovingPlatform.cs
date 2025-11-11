using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatfrom : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public Transform desPos;
    public float speed;

    private Vector3 lastPos;
    private bool playerOnPlatform = false;
    private Transform player;

    void Start()
    {
        transform.position = startPos.position;
        desPos = endPos;
        lastPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerOnPlatform = true;
            player = collision.transform;
            lastPos = transform.position; // 초기 위치 기록
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerOnPlatform = false;
            player = null;
        }
    }

    void FixedUpdate()
    {
        // 플랫폼 움직이기
        transform.position = Vector2.MoveTowards(
            transform.position,
            desPos.position,
            Time.deltaTime * speed
        );

        // 도달하면 방향 바꾸기
        if (Vector2.Distance(transform.position, desPos.position) <= 0.05f)
        {
            if (desPos == endPos) desPos = startPos;
            else desPos = endPos;
        }

        // 플레이어가 플랫폼 위에 있으면 이동량 적용
        if (playerOnPlatform && player != null)
        {
            Vector3 delta = transform.position - lastPos;
            player.position += delta;
        }

        // 마지막 위치 업데이트
        lastPos = transform.position;
    }
}
