using UnityEngine;

public class LightMoving : MonoBehaviour
{
    [SerializeField] private Transform movingTarget;   // 이동 타켓

    public float speed = 1f;            // 오브젝트 이동 속도
    public float amplitude = 0.15f;        // sin 곡선 진폭
    public float rotationSpeed = 15f;   // 오브젝트 회전 속도

    private float startTime;

    private void Start()
    {
        startTime = Time.time; // 시작 시간 기록
    }

    private void Update()
    {
        float xPos = Mathf.Sin((Time.time - startTime) * speed) * amplitude; // sin 곡선으로 x축 위치 계산
        movingTarget.localPosition = new Vector3(xPos, movingTarget.localPosition.y, movingTarget.localPosition.z); // 오브젝트의 위치 업데이트

        // 오브젝트를 회전
        float zRotation = Mathf.Sin((Time.time - startTime) * speed) * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
