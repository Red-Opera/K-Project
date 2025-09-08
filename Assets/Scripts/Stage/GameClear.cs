using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public float delayTime = 60f;  // 자동 이동까지 시간(초)
    private float timer;

    void OnEnable()
    {
        timer = 0f; // 활성화될 때 타이머 초기화
    }

    void Update()
    {
        // 특정 키 입력 체크
        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadNextScene();
            return;
        }

        // 시간 체크
        timer += Time.deltaTime;
        if (timer >= delayTime)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // 씬 이동
        SceneManager.LoadScene("Map");
    }
}
