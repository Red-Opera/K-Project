using UnityEngine;

public class LightMoving : MonoBehaviour
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
        float xPos = Mathf.Sin((Time.time - startTime) * speed) * amplitude;                            // sin ����� z�� ��ġ ���
        movingTarget.localPosition = new Vector3(xPos, movingTarget.localPosition.y, movingTarget.localPosition.z);    // ������Ʈ�� ��ġ ������Ʈ
    }
}
