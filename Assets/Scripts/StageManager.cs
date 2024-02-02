using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public string[] stageNames; // �������� �̸� �迭
    private int currentStageIndex = 0; // ���� �������� �ε���

    void Update()
    {
        // ���� ȭ��ǥ Ű�� ������ ���� ���������� �̵�
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadNextStage();
        }

        // �Ʒ��� ȭ��ǥ Ű�� ������ ���� ���������� �̵�
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LoadPreviousStage();
        }
    }

    // ���� ���������� �̵��ϴ� �Լ�
    void LoadNextStage()
    {
        currentStageIndex = (currentStageIndex + 1) % stageNames.Length;
        LoadStage(currentStageIndex);
    }

    // ���� ���������� �̵��ϴ� �Լ�
    void LoadPreviousStage()
    {
        currentStageIndex = (currentStageIndex - 1 + stageNames.Length) % stageNames.Length;
        LoadStage(currentStageIndex);
    }

    // ���������� �ε��ϴ� �Լ�
    void LoadStage(int stageIndex)
    {
        string stageName = stageNames[stageIndex];
        Debug.Log("Loading stage: " + stageName);

        // ���⿡ ���� �������� �ε��ϴ� �ڵ带 �߰��� �� �ֽ��ϴ�.
    }
}
