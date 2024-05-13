using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalActivation : MonoBehaviour
{
    FadeEffect fadeEffect;

    void Awake()
    {
        OnEnable();
    }
    void Start()
    {
        fadeEffect = GameObject.Find("Fade").GetComponent<FadeEffect>();
    }
    void Update()
    {
        CheckForMonsters();
    }

    private void CheckForMonsters()
    {
         // ���͸� ã���ϴ�.
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        // ���Ͱ� �ִ��� Ȯ���ϰ� ó���մϴ�.
        if (monsters.Length > 0)
        {
            Debug.Log("���Ͱ� �ֽ��ϴ�");

            // �ڽĵ� �߿��� "Next" ������Ʈ�� ã��
            Transform nextObject = transform.Find("Next");
            
            // "Next" ������Ʈ�� �����ϰ� Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
            if (nextObject != null && nextObject.gameObject.activeSelf)
            {
                // "Next" ������Ʈ�� ��Ȱ��ȭ
                nextObject.gameObject.SetActive(false);
            }

            // �ڽĵ� �߿��� "Previous" ������Ʈ�� ã��
            Transform previousObject = transform.Find("Previous");
            
            // "Previous" ������Ʈ�� �����ϰ� Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
            if (previousObject != null && previousObject.gameObject.activeSelf)
            {
                // "Previous" ������Ʈ�� ��Ȱ��ȭ
                previousObject.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("���Ͱ� �����ϴ�");

            // �ڽĵ� �߿��� "Next" ������Ʈ�� ã��
            Transform nextObject = transform.Find("Next");
            
            // "Next" ������Ʈ�� �����ϰ� ��Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
            if (nextObject != null && !nextObject.gameObject.activeSelf)
            {
                // "Next" ������Ʈ�� Ȱ��ȭ
                nextObject.gameObject.SetActive(true);
            }

            // �ڽĵ� �߿��� "Previous" ������Ʈ�� ã��
            Transform previousObject = transform.Find("Previous");
            
            // "Previous" ������Ʈ�� �����ϰ� ��Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
            if (previousObject != null && !previousObject.gameObject.activeSelf)
            {
                // "Previous" ������Ʈ�� Ȱ��ȭ
                previousObject.gameObject.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        fadeEffect = GameObject.Find("Fade").GetComponent<FadeEffect>();

        StartCoroutine(fadeEffect.FadeIn());
    }

    private IEnumerator StageChange ()
    {
        StartCoroutine(fadeEffect.FadeOut());
            while (fadeEffect.isFadeOut)
            {
                yield return null;
            }
        yield return new WaitForSeconds(0);
    }
}
