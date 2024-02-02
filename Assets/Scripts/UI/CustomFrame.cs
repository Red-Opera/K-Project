using System.Collections.Generic;
using UnityEngine;

public class CustomFrame : MonoBehaviour
{
    [SerializeField] private GameObject customFrames;               // Ŀ���� �������� ���� �ִ� ������Ʈ
    [SerializeField] private float scrollSpeed = 500;               // ���콺 ��ũ�� ���ǵ�

    private List<GameObject> frames = new List<GameObject>();       // �������� ������ �迭
    private List<Vector3> frameDefaultPos = new List<Vector3>();    // �������� �ʱ� ��ġ�� ������ �迭
    
    private float sumMouseWheel = 0;            // ���� ���콺 �� ����
    private float beforeSumMouseWheel = 0;      // ���� ���콺 �� ����
    private float frameWidth = 0;               // �� �������� �ʺ�

    void Start()
    {
        Debug.Assert(customFrames != null, "Ŀ�������� �� �� �ִ� �������� �����ϴ�.");

        // ��� �������� ������ ��ġ�� ������
        for (int i = 0; i < customFrames.transform.childCount; i++)
        {
            GameObject frame = customFrames.transform.GetChild(i).gameObject;

            frames.Add(frame);
            frameDefaultPos.Add(frame.transform.position);
        }

        // �������� �ʺ� �Է� ����
        frameWidth = frames[frames.Count - 1].GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        // ���콺 �� ������ �Է�
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        sumMouseWheel += mouseWheel * scrollSpeed;
        
        // �����ӵ��� ��� �������� ���� ���� ����
        if (sumMouseWheel > 0)
            sumMouseWheel = 0;

        float lastFrameRightPos = frameDefaultPos[frameDefaultPos.Count - 1].x + sumMouseWheel + frameWidth / 2;

        // �����ӵ��� ��� ���������� ���� ���� ����
        if ((mouseWheel * scrollSpeed < 0) && (lastFrameRightPos < 1137))
            sumMouseWheel = 1137 - frameDefaultPos[frameDefaultPos.Count - 1].x - frameWidth / 2;

        // ���� �������� ���� ���
        if (beforeSumMouseWheel == sumMouseWheel)
            return;

        // ���� �����ӿ� ���� �����ӵ� �� ��ġ��ŭ �̵��Ѱ����� ����
        for (int i = 0; i < frameDefaultPos.Count; i++)
            frames[i].transform.position = new Vector3(frameDefaultPos[i].x + sumMouseWheel, frameDefaultPos[i].y, frameDefaultPos[i].z);

        // �̵� ó��
        beforeSumMouseWheel = sumMouseWheel;
    }
}
