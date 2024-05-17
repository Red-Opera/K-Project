using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalActivation : MonoBehaviour
{
    FadeEffect fadeEffect;

    void Awake()
    {
        OnEnable(); //스테이지 넘어갔을 때 페이드 인 효과
    }
    void Start()
    {
        fadeEffect = GameObject.Find("Fade").GetComponent<FadeEffect>();
    }
    void Update()
    {
        CheckForMonsters();//스테이지에 몬스터 테그를 가진 오브젝트가 존재하는지 검사
    }

    private void CheckForMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        if (monsters.Length > 0) //몬스터가 있으면 비활성화
        {
            Transform nextObject = transform.Find("Next");
            
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
                previousObject.gameObject.SetActive(false);
            }
            
        }
        else // 몬스터가 없으면 활성화
        {
            // 자식들 중에서 "Next" 오브젝트를 찾음
            Transform nextObject = transform.Find("Next");
            
            // "Next" 오브젝트가 존재하고 비활성화되어 있는지 확인
            if (nextObject != null && !nextObject.gameObject.activeSelf)
            {
                // "Next" 오브젝트를 활성화
                nextObject.gameObject.SetActive(true);
            }

            Transform previousObject = transform.Find("Previous");
            
            if (previousObject != null && !previousObject.gameObject.activeSelf)
            {
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
