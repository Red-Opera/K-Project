using System.Security.Cryptography;
using UnityEngine;

public class UIOpenToKeyBoard : MonoBehaviour
{
    [SerializeField] private GameObject pressedKeyImage;    // �ö󰡴� �̹��� ������Ʈ
    [SerializeField] private AnimationCurve speedCurve;     // �ð��� ���� �߰��� �̵��� ��ġ�� ��Ÿ���� AnimationCurve
    [SerializeField] private Vector3 originalPosition;      // �̹����� ���� ��ġ
    [SerializeField] private GameObject openUI;             // UI ų ���

    private bool isRising = false;  // �ö󰡴� ������ ����
    private bool isFalling = false; // �������� ������ ����
    private bool isEnter = false;   // �÷��̾ ���Դ��� ����
    private float targetHeight;     // �ö󰡴� ��ǥ ����
    private float startTime;        // ���� ���� �ð�
    private float reverseTime;      // �ڷ� ���� �ð�

    public void Start()
    {
        Debug.Assert(pressedKeyImage != null, "������ �ϴ� Ű UI�� �����ϴ�.");
        originalPosition = pressedKeyImage.transform.position;              // ���� ��ġ ����
        pressedKeyImage.SetActive(false);                                   // ���� �� �̹��� ��Ȱ��ȭ
    }

    public void Update()
    {
        // �ö󰡴� ���̸�
        if (isRising)
        {
            if (reverseTime < 0)
                reverseTime = 0.0f;

            // ������� ����� �ð��� ���� ��ġ �� ���
            float timeSinceStart = Time.time - startTime + reverseTime;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // �̹����� õõ�� �ø�
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // ��ǥ ���̿� �����ϸ� �ö󰡴� ���� ����
            if (pressedKeyImage.transform.position.y >= targetHeight)
                isRising = false;
        }

        // �������� ���̸�
        if (isFalling)
        {
            // ������� ����� �ð��� ���� ��ġ �� ���
            float timeSinceStart = reverseTime + startTime - Time.time;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // �̹����� õõ�� ����
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // ���� ��ġ�� �����ϸ� �������� ���� ����
            if (pressedKeyImage.transform.position.y <= targetHeight)
            {
                isFalling = false;
                pressedKeyImage.transform.position = originalPosition; // ���� ��ġ�� �̵�
            }
        }

        if (isEnter && !openUI.activeSelf && Input.GetKeyDown(KeyCode.P))
            openUI.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �浹�� ������Ʈ�� �÷��̾��� ��� �̹����� Ȱ��ȭ�ϰ� �ö󰡱� ����
            pressedKeyImage.SetActive(true);

            isRising = true;
            isFalling = false;  // �������� ������ ���� ���� �ƴϵ��� ����
            isEnter = true;     // �÷��̾ �������� ǥ��

            targetHeight = pressedKeyImage.transform.position.y + speedCurve.keys[speedCurve.length - 1].value;   // ��ǥ ���� ����
            reverseTime = reverseTime + startTime - Time.time;          // ���� ���� �ð� ����
            startTime = Time.time;                                      // ���� ���� �ð� ����
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ Ʈ���� ������ ���������� ���� �������� ���� ����
            isRising = false;
            isFalling = true;
            isEnter = false;

            targetHeight = originalPosition.y;      // ��ǥ ���̸� ���� ��ġ�� ����
            reverseTime = Time.time - startTime;    // ���� ���� �ð� ����
            startTime = Time.time;
        }
    }
}