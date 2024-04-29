using UnityEngine;

public class LightMoving : MonoBehaviour
{
    [SerializeField] private Transform movingTarget;   // 이동 타켓

    public float speed = 5f;        // 오브젝트 이동 속도
    public float amplitude = 5f;    // sin 곡선 진폭

    private float startTime;

    private void Start()
    {
        startTime = Time.time; // 시작 시간 기록
    }

    private void Update()
    {
        float xPos = Mathf.Sin((Time.time - startTime) * speed) * amplitude;                            // sin 곡선으로 z축 위치 계산
        movingTarget.localPosition = new Vector3(xPos, movingTarget.localPosition.y, movingTarget.localPosition.z);    // 오브젝트의 위치 업데이트
    }
}
