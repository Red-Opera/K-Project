using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LoadingCamera : MonoBehaviour
{
    private Camera loadingCamera;
    private GameObject mainCamera;
    private GameObject eventSystem;
    private GameObject fade;

    private void OnEnable()
    {
        loadingCamera = GetComponent<Camera>();
        Debug.Assert(loadingCamera != null, "Error (Null Reference) : �ε� ī�޶� �������� �ʽ��ϴ�.");

        eventSystem = GameObject.Find("EventSystem");
        fade = GameObject.Find("Fade");
        mainCamera = GameObject.Find("Main Camera");

        mainCamera.GetComponent<Camera>().GetUniversalAdditionalCameraData().cameraStack.Add(loadingCamera);

        eventSystem.SetActive(false);
        fade.SetActive(false);
    }

    private void OnDisable()
    {
        if (mainCamera == null)
            mainCamera = GameObject.Find("Main Camera");

        mainCamera.GetComponent<Camera>().GetUniversalAdditionalCameraData().cameraStack.Remove(loadingCamera);
    }
}
