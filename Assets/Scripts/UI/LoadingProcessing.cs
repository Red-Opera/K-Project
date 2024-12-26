using UnityEngine;
using UnityEngine.Rendering;

public class LoadingProcessing : MonoBehaviour
{
    private Volume loadingProcess;    // �� ����Ʈ���μ���
    private GameObject loadingUI;                // �ε� UI  

    void Start()
    {
        loadingProcess = GetComponent<Volume>();
        loadingUI = GameObject.Find("Loading").transform.GetChild(0).gameObject;

        Debug.Assert(loadingProcess != null, "���� ����Ʈ ���μ����� �����ϴ�.");
        Debug.Assert(loadingUI != null, "�ε� UI�� �����ϴ�.");
    }

    void Update()
    {
        if (loadingUI.activeSelf)
            loadingProcess.weight = 0;
        
        else
            loadingProcess.weight = 1;
    }
}
