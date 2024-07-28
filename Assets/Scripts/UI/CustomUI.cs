using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class CustomUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> players;  // �÷��̾� ������Ʈ
    [SerializeField] private Transform frames;          // ĳ���� �������� ��� ������Ʈ

    private static int currentPlayerIndex = 2;          // ���� �÷��̾� �ε��� ��
    private bool isChange = false;                      // ĳ���͸� �ٲ� ���

    private void Start()
    {
        Debug.Assert(frames != null, "ĳ���� �������� ��� ������Ʈ�� �����ϴ�.");
    }

    private void Update()
    {
        // ĳ���͸� ������ ���
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            ChangePlayer();
    }

    // �÷��̾ ��ü���ִ� �޼ҵ�
    private void ChangePlayer()
    {
        for (int i = 0; i < frames.childCount; i++)
        {
            // �ش� ��ȣ�� �����ӿ� �̹����� ������
            Transform frame = frames.GetChild(i).GetChild(0);
            Image frameImage = frame.GetComponent<Image>();

            // �ش� �̸��� �������� ���õ� ���
            if (frameImage.color.a >= 0.98)
            {
                // �̹� ������ �÷��̾��� ���
                if (i == currentPlayerIndex)
                    break;

                // ���� ������ �÷��̾� �ε����� �ݿ��ϰ� �÷��̾ �ٲ��� �˸�
                currentPlayerIndex = i;
                isChange = true;

                // ���� �÷��̾�� ī�޶�, ��ġ�� ������
                GameObject destroyPlayer = GameObject.FindGameObjectWithTag("Player");
                Camera mainCamera = destroyPlayer.transform.GetChild(0).GetComponent<Camera>();
                Vector2 defaultPlayerPos = new Vector2(destroyPlayer.transform.position.x, destroyPlayer.transform.localPosition.y);

                // ������ �÷��̾ ����� ī�޶� �ٲٰ� Ȱ��ȭ �� �÷��̾� ��ġ�� ���� �÷��̾� ��ġ�� �ٲ�
                GameObject newPlayer = Instantiate(players[i]);
                Camera toCamera = newPlayer.transform.GetChild(0).GetComponent<Camera>();
                newPlayer.SetActive(true);
                newPlayer.transform.position = defaultPlayerPos;

                // ī�޶� ������ ���� ������
                List<Camera> defualtCameraStack = mainCamera.GetUniversalAdditionalCameraData().cameraStack;
                List<Camera> toCameraStack = toCamera.GetUniversalAdditionalCameraData().cameraStack;

                // ī�޶� ������ ������
                for (int j = 0; j < defualtCameraStack.Count; j++)
                    toCameraStack.Add(defualtCameraStack[j]);

                newPlayer.GetComponent<PMove>().FindUI();
                
                DontDestroyOnLoad(newPlayer);
                Destroy(destroyPlayer);
            }
        }

        // �ٲ���ٸ� UI�� ��
        if (isChange)
            gameObject.SetActive(false);
    }
}
