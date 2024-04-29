using UnityEngine;

public class LampMoving : MonoBehaviour
{
    [SerializeField] private Transform movingTarget;   // �̵� Ÿ��

    public float speed = 5f;        // ������Ʈ �̵� �ӵ�
    public float amplitude = 5f;    // sin � ����

    private float startTime;

    private void Start()
    {
        startTime = Time.time; // ���� �ð� ���
    }

    private void Update()
    {
        float angle = Mathf.Sin((Time.time - startTime) * speed) * amplitude;  // sin ����� ���� ���
        movingTarget.localEulerAngles = new Vector3(0f, 0f, angle); // ������Ʈ�� ���� ������Ʈ
    }
}
