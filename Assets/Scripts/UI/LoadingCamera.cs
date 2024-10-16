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
        Debug.Assert(loadingCamera != null, "Error (Null Reference) : 로딩 카메라가 존재하지 않습니다.");

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
