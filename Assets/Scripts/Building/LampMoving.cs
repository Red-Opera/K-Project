using UnityEngine;

public class LampMoving : MonoBehaviour
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
        float angle = Mathf.Sin((Time.time - startTime) * speed) * amplitude;  // sin 곡선으로 각도 계산
        movingTarget.localEulerAngles = new Vector3(0f, 0f, angle); // 오브젝트의 각도 업데이트
    }
}
