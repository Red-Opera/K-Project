using TMPro;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private GameObject states; // ���¸� ������ �� �ִ� UI ����

    void Start()
    {
        Debug.Assert(states != null, "���¸� ����� â�� �����ϴ�.");
    }

    private void OnEnable()
    {
        // ���� ���¸� ������Ʈ���� �ʾ��� ���
        if (State.datas.Count == 0)
            return;

        for (int i = 0; i < states.transform.childCount; i++)
        {
            // ������ ������Ʈ�� �̸��� ������
            string stateName = states.transform.GetChild(i).gameObject.name;

            // �� ���°� �����ϴ��� ã�ƺ�
            object fieldValue = null;
            try { fieldValue = State.datas[stateName].GetValue(GameManager.info.allPlayerState); }

            catch { continue; }

            if (fieldValue == null)
                continue;

            // �߰� �۾��� ���� ���
            AddWork(stateName, ref fieldValue);

            states.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = fieldValue.ToString();
        }
    }

    // �߰� �۾��� �ϴ� �޼ҵ�
    private void AddWork(string stateName, ref object fieldValue)
    {
        // �� �̸����� �ۼ�Ʈ�� �ٲ�
        if (stateName == "Critical" || stateName == "DashDamage" || stateName == "AvoidPersent" || stateName == "DefensePersent" || stateName == "AttackSpeed")
        {
            float value = (float)fieldValue * 100;
            string valueString = value.ToString() + "%";

            fieldValue = valueString;
        }
    }
}