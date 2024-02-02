using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private State state;       // 플레이어 상태
    [SerializeField] private GameObject states; // 상태를 저장할 수 있는 UI 모음

    void Start()
    {
        Debug.Assert(state != null, "플레이어 상태가 없습니다.");
        Debug.Assert(states != null, "상태를 출력할 창이 없습니다.");
    }

    private void OnEnable()
    {
        // 아직 상태를 업데이트하지 않았을 경우
        if (State.datas.Count == 0)
            return;

        for (int i = 0; i < states.transform.childCount; i++)
        {
            // 전시할 오브젝트의 이름을 가져옴
            string stateName = states.transform.GetChild(i).gameObject.name;

            // 그 상태가 존재하는지 찾아봄
            object fieldValue = null;
            try { fieldValue = State.datas[stateName].GetValue(state); }

            catch { continue; }

            if (fieldValue == null)
                continue;

            // 추가 작업이 있을 경우
            AddWork(stateName, ref fieldValue);

            states.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = fieldValue.ToString();
        }
    }

    // 추가 작업을 하는 메소드
    private void AddWork(string stateName, ref object fieldValue)
    {
        // 위 이름들은 퍼센트로 바꿈
        if (stateName == "Critical" || stateName == "CriticalDamage" || stateName == "AvoidPersent" || stateName == "DefensePersent")
        {
            float value = (float)fieldValue * 100;
            string valueString = value.ToString() + "%";

            fieldValue = valueString;
        }
    }
}