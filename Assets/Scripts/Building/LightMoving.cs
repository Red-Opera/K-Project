using UnityEngine;

public class LightMoving : MonoBehaviour
{
    [SerializeField] private Transform movingTarget;   // �̵� Ÿ��

    public float speed = 1f;            // ������Ʈ �̵� �ӵ�
    public float amplitude = 0.15f;        // sin � ����
    public float rotationSpeed = 15f;   // ������Ʈ ȸ�� �ӵ�

    private float startTime;

    private void Start()
    {
        startTime = Time.time; // ���� �ð� ���
    }

    private void Update()
    {
        float xPos = Mathf.Sin((Time.time - startTime) * speed) * amplitude; // sin ����� x�� ��ġ ���
        movingTarget.localPosition = new Vector3(xPos, movingTarget.localPosition.y, movingTarget.localPosition.z); // ������Ʈ�� ��ġ ������Ʈ

        // ������Ʈ�� ȸ��
        float zRotation = Mathf.Sin((Time.time - startTime) * speed) * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
