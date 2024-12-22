using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minimap : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] private TextMeshProUGUI multiple;      // ������ ǥ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI sceneNameText; // �� �̸��� ǥ���� �ؽ�Ʈ

    [SerializeField] private float scaleFactor;             // ������ ������ ����
    [SerializeField] private float maxDistance;             // �ִ�� ���������� �Ÿ�
    [SerializeField] private float minDistance;             // �ּҷ� ���������� �Ÿ�

    private Camera cameraCompo;

    void Start()
    {
        Debug.Assert(camera != null, "�̴ϸ� ��� ī�޶� �������� �ʽ��ϴ�.");

        cameraCompo = camera.GetComponent<Camera>();
        Debug.Assert(cameraCompo != null, "ī�޶� ������Ʈ�� �������� �ʽ��ϴ�.");

        multiple.text = (20 / cameraCompo.orthographicSize).ToString("X#0.#");

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Challenger")
            transform.parent.parent.gameObject.SetActive(false);

        if (sceneName != "Map")
            sceneNameText.text = sceneName;
    }

    void Update()
    {
        camera.transform.position = Camera.main.transform.parent.position + new Vector3(0, 0, -10);

        MinimapScale();
    }

    private void MinimapScale()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float multipleValue = 20 / cameraCompo.orthographicSize;

        // ���� �Ʒ��� ��ũ���� ��
        if (scroll < 0 && cameraCompo.orthographicSize <= maxDistance)
        {
            cameraCompo.orthographicSize += scaleFactor;   // ������ ���͸�ŭ ī�޶� ���� �̵�

            multiple.text = multipleValue.ToString("X#0.#");
        }

        // ���� ���� ��ũ���� ��
        else if (scroll > 0 && cameraCompo.orthographicSize >= minDistance)
        {
            cameraCompo.orthographicSize -= scaleFactor;   // ������ ���͸�ŭ ī�޶� �Ʒ��� �̵�

            multiple.text = multipleValue.ToString("X#0.#");
        }
    }
}
