using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OverlayCamera : MonoBehaviour
{
    // �ڵ����� ���� ī�޶� ���ÿ� ����
    void Start()
    {
        Camera camera = Camera.main;

        camera.GetUniversalAdditionalCameraData().cameraStack.Add(GetComponent<Camera>());
    }
}
