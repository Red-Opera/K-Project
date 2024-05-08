using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OverlayCamera : MonoBehaviour
{
    private bool isAdd = false;

    // 자동으로 메인 카메라 스택에 넣음
    private void Start()
    {
        AddCamera();
    }

    public void AddCamera()
    {
        if (isAdd)
            return;

        isAdd = true;
        Camera camera = Camera.main;

        camera.GetUniversalAdditionalCameraData().cameraStack.Add(GetComponent<Camera>());
    }
}
