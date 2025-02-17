using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minimap : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] private TextMeshProUGUI multiple;      // 배율을 표시할 텍스트
    [SerializeField] private TextMeshProUGUI sceneNameText; // 씬 이름을 표시할 텍스트

    [SerializeField] private float scaleFactor;             // 조절할 스케일 팩터
    [SerializeField] private float maxDistance;             // 최대로 조절가능한 거리
    [SerializeField] private float minDistance;             // 최소로 조절가능한 거리

    private Camera cameraCompo;

    void Start()
    {
        Debug.Assert(camera != null, "미니맵 출력 카메라가 존재하지 않습니다.");

        cameraCompo = camera.GetComponent<Camera>();
        Debug.Assert(cameraCompo != null, "카메라 컴포넌트가 존재하지 않습니다.");

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

        // 휠을 아래로 스크롤할 때
        if (scroll < 0 && cameraCompo.orthographicSize <= maxDistance)
        {
            cameraCompo.orthographicSize += scaleFactor;   // 스케일 팩터만큼 카메라를 위로 이동

            multiple.text = multipleValue.ToString("X#0.#");
        }

        // 휠을 위로 스크롤할 때
        else if (scroll > 0 && cameraCompo.orthographicSize >= minDistance)
        {
            cameraCompo.orthographicSize -= scaleFactor;   // 스케일 팩터만큼 카메라를 아래로 이동

            multiple.text = multipleValue.ToString("X#0.#");
        }
    }
}
