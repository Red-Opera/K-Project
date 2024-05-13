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
         // 몬스터를 찾습니다.
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        // 몬스터가 있는지 확인하고 처리합니다.
        if (monsters.Length > 0)
        {
            Debug.Log("몬스터가 있습니다");

            // 자식들 중에서 "Next" 오브젝트를 찾음
            Transform nextObject = transform.Find("Next");
            
            // "Next" 오브젝트가 존재하고 활성화되어 있는지 확인
            if (nextObject != null && nextObject.gameObject.activeSelf)
            {
                // "Next" 오브젝트를 비활성화
                nextObject.gameObject.SetActive(false);
            }

            // 자식들 중에서 "Previous" 오브젝트를 찾음
            Transform previousObject = transform.Find("Previous");
            
            // "Previous" 오브젝트가 존재하고 활성화되어 있는지 확인
            if (previousObject != null && previousObject.gameObject.activeSelf)
            {
                // "Previous" 오브젝트를 비활성화
                previousObject.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("몬스터가 없습니다");

            // 자식들 중에서 "Next" 오브젝트를 찾음
            Transform nextObject = transform.Find("Next");
            
            // "Next" 오브젝트가 존재하고 비활성화되어 있는지 확인
            if (nextObject != null && !nextObject.gameObject.activeSelf)
            {
                // "Next" 오브젝트를 활성화
                nextObject.gameObject.SetActive(true);
            }

            // 자식들 중에서 "Previous" 오브젝트를 찾음
            Transform previousObject = transform.Find("Previous");
            
            // "Previous" 오브젝트가 존재하고 비활성화되어 있는지 확인
            if (previousObject != null && !previousObject.gameObject.activeSelf)
            {
                // "Previous" 오브젝트를 활성화
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
