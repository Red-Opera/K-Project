using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    private List<GameObject> stageMap;  // 스테이지 별 맵 UI를 담는 배열

    private Transform map;              // 맵 오브젝트

    public void OnEnable()
    {
        // 처음 활성화할 경우
        if (stageMap == null)
        {
            stageMap = new List<GameObject>();

            // 모든 Stage 맵 정보를 가져옴
            Transform stageMaps = transform.GetChild(0);
            for (int i = 0; i < stageMaps.childCount; i++)
                stageMap.Add(stageMaps.GetChild(i).gameObject);
        }

        // 현재 열려있는 레벨 정보를 가져옴 
        int level = OnStageUI();

        if (level <= 0)
            return;

        // 맵 오브젝트를 가져옴
        map = GameObject.Find(level.ToString() + "Stages").transform;

        // 현재 스테이지에 해당하는 UI를 활성화 함
        CurrentStageEnable(level);
    }

    // 모든 레벨 UI를 비활성화하는 메소드
    private void MapAllDisable()
    {
        for (int i = 0; i < stageMap.Count; i++)
            stageMap[i].SetActive(false);
    }

    // 현재 스테이지를 활성화시키는 메소드
    private void CurrentStageEnable(int level)
    {
        // 현재 레벨 정보를 가져옴
        Transform currentLevel = stageMap[level].transform;

        // 모든 스테이지를 비활성화
        for (int i = 1; i < currentLevel.childCount - 1; i++)
            currentLevel.GetChild(i).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);

        // 현재 활성화되어 있는 스테이지는 활성화되도록 설정
        for (int i = 0; i < map.childCount; i++)
        {
            GameObject stage = map.GetChild(i).gameObject;

            // 해당 스테이지가 활성화되어 있는 경우
            if (stage.activeSelf)
            {
                // 보스 스테이지인 경우
                if (stage.name.Contains("BossRoom"))
                {
                    Transform bossStageUI = currentLevel.Find("BossStage");
                    bossStageUI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);

                    break;
                }

                // 이벤트 스테이지가 활성화 되어있는 경우
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
