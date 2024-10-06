using System.Collections;
using UnityEngine;

public class UIOpenToKeyBoard : MonoBehaviour
{
    [SerializeField] private GameObject pressedKeyImage;    // Ű�� ������ �� ǥ�õǴ� �̹���
    [SerializeField] private AnimationCurve speedCurve;     // �̹����� �����̴� �ӵ��� �����ϴ� AnimationCurve
    [SerializeField] private Vector3 originalPosition;      // �̹����� ���� ��ġ
    [SerializeField] private GameObject openUI;             // ���� UI â

    private ChatNPC chatNPC;        // NPC�� ��ȭ�ϴ� ����� ���� ��ũ��Ʈ ����

    private bool isRising = false;  // �̹����� ��� ������ ����
    private bool isFalling = false; // �̹����� �ϰ� ������ ����
    private bool isEnter = false;   // �÷��̾ Ʈ���� ������ �ִ��� ����
    private float targetHeight;     // �̹����� ������ ��ǥ ����
    private float startTime;        // �ִϸ��̼� ���� �ð�
    private float reverseTime;      // ������ �ִϸ��̼� �ð�
    private PMove pMove;
    private WeaponController weaponController;

    public void Start()
    {
        Debug.Assert(pressedKeyImage != null, "Ű �̹����� �������� �ʾҽ��ϴ�.");
        originalPosition = pressedKeyImage.transform.position;              // �̹����� ���� ��ġ ����
        pressedKeyImage.SetActive(false);                                   // ó������ �̹����� ������ �ʵ��� ����

        chatNPC = GetComponent<ChatNPC>();
    }

    public void Update()
    {
        // �̹����� ��� ���� ��
        if (isRising)
        {
            pressedKeyImage.SetActive(true);

            if (reverseTime < 0)
                reverseTime = 0.0f;

            // �ִϸ��̼��� ����� �ð��� ���� ���ο� ���� ���
            float timeSinceStart = Time.time - startTime + reverseTime;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // �̹����� ���ο� ��ġ ����
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // ��ǥ ���̿� �����ϸ� ��� ���� ����
            if (pressedKeyImage.transform.position.y >= targetHeight)
                isRising = false;
        }

        // �̹����� �ϰ� ���� ��
        if (isFalling)
        {
            // �ִϸ��̼��� ����� �ð��� ���� ���ο� ���� ���
            float timeSinceStart = reverseTime + startTime - Time.time;
            float newHeight = originalPosition.y + speedCurve.Evaluate(timeSinceStart);

            // �̹����� ���ο� ��ġ ����
            pressedKeyImage.transform.position = new Vector3(pressedKeyImage.transform.position.x, newHeight, pressedKeyImage.transform.position.z);

            // ���� ��ġ�� �����ϸ� �ϰ� ���� ����
            if (pressedKeyImage.transform.position.y <= targetHeight)
            {
                isFalling = false;
                pressedKeyImage.SetActive(false);
                pressedKeyImage.transform.position = originalPosition; // ���� ��ġ�� ����
            }
        }

        // �÷��̾ Ʈ���� ������ �ְ�, UI�� ������ �ʾ�����, FŰ�� ������ ��
        if (isEnter && !openUI.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            if (chatNPC != null && !chatNPC.dialog.isChat)
            {
                chatNPC.Chat("BlackSmith");
                pMove.SendDialog(chatNPC.dialog);
                weaponController.SendDialog(chatNPC.dialog);
                StartCoroutine(WaitChat());
            }
            else
            {
                openUI.SetActive(true);
                pMove.SendUI(openUI);
                weaponController.SendUI(openUI);
                Debug.Log(openUI.tag);
            }
        }
    }

    private IEnumerator WaitChat()
    {
        while (chatNPC.dialog.isChat)
            yield return null;

        openUI.SetActive(true);
        pMove.SendUI(openUI);
        weaponController.SendUI(openUI);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pMove = other.GetComponent<PMove>();
            weaponController = other.GetComponent<WeaponController>();
            // �÷��̾ Ʈ���ſ� ������ �� ��� ����
            pressedKeyImage.SetActive(true);

            isRising = true;
            isFalling = false;  // �ϰ� ���� �ƴ϶�� ���·� ����
            isEnter = true;     // �÷��̾ ������ ������ ǥ��

            targetHeight = pressedKeyImage.transform.position.y + speedCurve.keys[speedCurve.length - 1].value;   // ��ǥ ���� ����
            reverseTime = reverseTime + startTime - Time.time;          // �ִϸ��̼� ���� �ð��� ���
            startTime = Time.time;                        // ���� �ð��� ���� �ð����� ����
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾ �������� ����� �ϰ� ����
            isRising = false;
            isFalling = true;
            isEnter = false;

            targetHeight = originalPosition.y;      // ��ǥ ���̸� ���� ��ġ�� ����
            reverseTime = Time.time - startTime;    // �ִϸ��̼� �ð� ���
            startTime = Time.time;
        }
    }
}
