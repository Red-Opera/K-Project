using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    private List<GameObject> stageMap;  // �������� �� �� UI�� ��� �迭

    private Transform map;              // �� ������Ʈ

    public void OnEnable()
    {
        // ó�� Ȱ��ȭ�� ���
        if (stageMap == null)
        {
            stageMap = new List<GameObject>();

            // ��� Stage �� ������ ������
            Transform stageMaps = transform.GetChild(0);
            for (int i = 0; i < stageMaps.childCount; i++)
                stageMap.Add(stageMaps.GetChild(i).gameObject);
        }

        // ���� �����ִ� ���� ������ ������ 
        int level = OnStageUI();

        if (level <= 0)
            return;

        // �� ������Ʈ�� ������
        map = GameObject.Find(level.ToString() + "Stages").transform;

        // ���� ���������� �ش��ϴ� UI�� Ȱ��ȭ ��
        CurrentStageEnable(level);
    }

    // ��� ���� UI�� ��Ȱ��ȭ�ϴ� �޼ҵ�
    private void MapAllDisable()
    {
        for (int i = 0; i < stageMap.Count; i++)
            stageMap[i].SetActive(false);
    }

    // ���� ���������� Ȱ��ȭ��Ű�� �޼ҵ�
    private void CurrentStageEnable(int level)
    {
        // ���� ���� ������ ������
        Transform currentLevel = stageMap[level].transform;

        // ��� ���������� ��Ȱ��ȭ
        for (int i = 1; i < currentLevel.childCount - 1; i++)
            currentLevel.GetChild(i).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);

        // ���� Ȱ��ȭ�Ǿ� �ִ� ���������� Ȱ��ȭ�ǵ��� ����
        for (int i = 0; i < map.childCount; i++)
        {
            GameObject stage = map.GetChild(i).gameObject;

            // �ش� ���������� Ȱ��ȭ�Ǿ� �ִ� ���
            if (stage.activeSelf)
            {
                // ���� ���������� ���
                if (stage.name.Contains("BossRoom"))
                {
                    Transform bossStageUI = currentLevel.Find("BossStage");
                    bossStageUI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);

                    break;
                }

                // �̺�Ʈ ���������� Ȱ��ȭ �Ǿ��ִ� ���
                if (stage.name.Contains("EventStage"))
                {
                    int eventLevel = int.Parse(stage.name[10..].ToString());

                    Transform eventLevelUI = currentLevel.Find("???" + eventLevel.ToString());
                    eventLevelUI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);

                    break;
                }

                if (stage.name.Contains("BoxStage"))
                {
                    Transform boxStage = currentLevel.Find(stage.name);
                    boxStage.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);

                    break;
                }

                int stageIndex = int.Parse(stage.name[5..].ToString());

                currentLevel.GetChild(stageIndex).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);

                break;
            }
        }
    }

    private int OnStageUI()
    {
        MapAllDisable();

        string mapName = SceneManager.GetActiveScene().name.ToString();

        if (mapName == "Map")
        {
            stageMap[0].SetActive(true);
            return 0;
        }

        else if (mapName.Contains("Stage"))
        {
            int level = int.Parse(mapName[5..].ToString());

            stageMap[level].SetActive(true);

            return level;
        }

        return -1;
    }
}
