using UnityEngine;
using UnityEngine.Rendering;

public class LoadingProcessing : MonoBehaviour
{
    private Volume loadingProcess;    // 맵 포스트프로세싱
    private GameObject loadingUI;                // 로딩 UI  

    void Start()
    {
        loadingProcess = GetComponent<Volume>();
        loadingUI = GameObject.Find("Loading").transform.GetChild(0).gameObject;

        Debug.Assert(loadingProcess != null, "맵의 포스트 프로세싱이 없습니다.");
        Debug.Assert(loadingUI != null, "로딩 UI가 없습니다.");
    }

    void Update()
    {
        if (loadingUI.activeSelf)
            loadingProcess.weight = 0;
        
        else
            loadingProcess.weight = 1;
    }
}
