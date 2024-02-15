using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INNChangeItem : MonoBehaviour
{
    [SerializeField] private GameObject[] foodList;     // ������ ������ �� �ִ� ����Ʈ
    [SerializeField] private Sprite[] selectableList;   // ���� ������ ���� ����Ʈ
    [SerializeField] private FoodState[] foodInfo;      // ���� ���� ����Ʈ

    [SerializeField] private float range = 0.2f;        // ���� �� ȿ�� ����

    void Start()
    {
        Debug.Assert(foodList.Length != 0, "������ ������ �� �ִ� �������� �����ϴ�.");

    }

    public void OnEnable()
    {
        for (int i = 0; i < foodList.Length; i++)
        {
            // ���� �̹��� �� �������� �ϳ� ����
            int foodIndex = Random.Range(0, selectableList.Length);

            // ������ �̹����� �̸��� �����ϰ� �̸��� ǥ���� UI�� �̹����� ������ ���� �̹����� ��ȯ
            string selectedName = selectableList[foodIndex].texture.name;
            foodList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = selectedName;
            foodList[i].transform.GetChild(2).GetComponent<Image>().sprite = selectableList[foodIndex];

            // �������� ������ ������ ���� ���� �迭���� ã��
            FoodState food = null;
            for (int j = 0; j < foodInfo.Length; j++)
            {
                if (foodInfo[j].foodName == selectedName)
                {
                    food = foodInfo[j];
                    break;
                }

                // ���� ������ ã�� �� ���� ���
                if (j == foodList.Length - 1)
                    Debug.Assert(false, "�ش� �̸��� ���� ������ �������� �ʽ��ϴ�.");
            }
            
            int addStateCount = food.addState.Count;                // �߰� ȿ�� ������ ������
            Transform addList = foodList[i].transform.GetChild(3);  // �߰� ������ ǥ���� ��ġ

            AddStateTurnOff(addList);

            // �߰� ȿ�� ���� ��ŭ �ݺ�
            for (int j = 0; j < addStateCount; j++)
            {
                // ���� ������ ǥ���� UI�� ��� Ȱ��ȭ
                for (int k = 0; k < 3; k++)
                    addList.GetChild(k).GetChild(j).gameObject.SetActive(true);

                // �߰� ���� �̸��� ��ġ�� UI�� �����
                addList.GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>().text = food.addState[j].stateName;
                ShowValue(addList.GetChild(2).GetChild(j).GetComponent<TextMeshProUGUI>(), food.addState[j].value);
            }
        }
    }

    private void AddStateTurnOff(Transform addList)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                addList.GetChild(j).GetChild(i).gameObject.SetActive(false);
    }

    private void ShowValue(TextMeshProUGUI outText, float value)
    {
        float addRange = Random.Range(-range, range);

        value += addRange * value;

        if (value >= 0)
        {
            outText.color = Color.green;
            outText.text = "+";
        }

        else
        {
            outText.color = Color.red;
            outText.text = "";
        }

        if (Mathf.Abs(value) < 0.1f)
            outText.text += value.ToString("#,##0.##");

        else
            outText.text += value.ToString("#,##0.#");
    }
}
