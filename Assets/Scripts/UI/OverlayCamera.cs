using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OverlayCamera : MonoBehaviour
{
    // 자동으로 메인 카메라 스택에 넣음
    void Start()
    {
        Camera camera = Camera.main;

        camera.GetUniversalAdditionalCameraData().cameraStack.Add(GetComponent<Camera>());
    }
}
