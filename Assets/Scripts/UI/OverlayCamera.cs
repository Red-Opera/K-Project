using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OverlayCamera : MonoBehaviour
{
    private bool isAdd = false;

    // �ڵ����� ���� ī�޶� ���ÿ� ����
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
