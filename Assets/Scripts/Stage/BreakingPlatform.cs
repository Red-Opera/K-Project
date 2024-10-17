using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    private Renderer platformRenderer;
    private Collider platformCollider;

    private void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // "Player" 태그가 있는 오브젝트가 발판에 닿으면
        {
            StartCoroutine(HandlePlatform());
        }
    }

    private IEnumerator HandlePlatform()
    {
        // 발판을 사라지게 함
        platformRenderer.enabled = false;
        platformCollider.enabled = false;

        // 3초 대기
        yield return new WaitForSeconds(3f);

        // 발판을 다시 나타나게 함
        platformRenderer.enabled = true;
        platformCollider.enabled = true;

        // 2초 대기
        yield return new WaitForSeconds(2f);
    }
}
