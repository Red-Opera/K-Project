using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class INNChangeItem : MonoBehaviour
{
    [SerializeField] private GameObject[] foodList;     // ������ ������ �� �ִ� ����Ʈ
    [SerializeField] private Sprite[] selectableList;   // ���� ������ ���� ����Ʈ
    [SerializeField] private FoodState[] foodInfo;      // ���� ���� ����Ʈ
    [SerializeField] private AudioClip openSound;       // ���� ���� �� �Ҹ�     

    [SerializeField] private float range = 0.2f;        // ���� �� ȿ�� ����

    private AudioSource audio;      // �Ҹ��� ����ϴ� ������Ʈ
    private float addRange;

    private void OnEnable()
    {
        if (audio == null)
        {
            Debug.Assert(foodList.Length != 0, "������ ������ �� �ִ� �������� �����ϴ�.");

            // ������ ���� �ڽ��� ���°���� Ȯ��
            for (int i = 0; i < foodList.Length; i++)
                foodList[i].GetComponent<FoodItem>().index = i;

            audio = GetComponent<AudioSource>();
            Debug.Assert(audio, "����� ������Ʈ�� �����ϴ�.");
        }

        audio.PlayOneShot(openSound);
        BackgroundSound.StartOtherCilp();

        // ��� ������ Ű�� ���� �ؽ�Ʈ�� ��
        for (int i = 0; i < foodList.Length; i++)
        {
            int childCount = foodList[i].transform.childCount;

            for (int j = 0; j < childCount; j++)
                foodList[i].transform.GetChild(j).gameObject.SetActive(true);

            foodList[i].transform.GetChild(childCount - 1).gameObject.SetActive(false);
            foodList[i].GetComponent<FoodItem>().isEnable = true;
        }

        StartCoroutine(DefaultSetting());
    }

    private void OnDisable()
    {
        BackgroundSound.StopOtherCilp();
    }

    private IEnumerator DefaultSetting()
    {
        // ���� �޴����� ���ٸ� ��� ���
        int failCount = 0;
        while (GameManager.info == null && failCount < 100)
        {
            failCount++;
            yield return null;
        }

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

            // �ش� ������ �߰ſ� �������� �˷���
            foodList[i].GetComponent<FoodItem>().isHotFood = food.isHotFood;

            int addStateCount = food.addState.Count;                // �߰� ȿ�� ������ ������
            Transform addList = foodList[i].transform.GetChild(3);  // �߰� ������ ǥ���� ��ġ

            addRange = Random.Range(-range, range);     // ��ǰ�� ���� ��ġ�� ����
            SetEfficiency(i);

            AddStateTurnOff(addList);                   // ��� �߰� ���¸� ��

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

            // �⺻ ������ ǥ���� ��ġ
            Transform baseList = foodList[i].transform.GetChild(4);

            // �߰� ȿ�� ���� ��ŭ �ݺ�
            for (int j = 0; j < 3; j++)
            {
                // �߰� ���� �̸��� ��ġ�� UI�� �����
                if (j != 2)
                    ShowValue(baseList.GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>(), food.baseState[j].value, true);

                else
                    ShowValue(baseList.GetChild(1).GetChild(j).GetComponent<TextMeshProUGUI>(), food.baseState[j].value, true, true);
            }
        }
    }

    // ���� ������ ��� ���� �޼ҵ�
    private void AddStateTurnOff(Transform addList)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                addList.GetChild(j).GetChild(i).gameObject.SetActive(false);
    }

    private void ShowValue(TextMeshProUGUI outText, float value, bool isBase = false, bool isMoney = false)
    {
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

        // ���� ��� ���� ������ �ִ� �翡 ���� ������ �ٲٰ� ������ ������ ����
        if (isMoney)
        {
            outText.color = (GameManager.info.playerState.money - (int)value >= 0) ? Color.green : Color.red;
            outText.text = "-";
        }

        // �⺻ ������ ��� ��� ���������� ǥ��
        if (isBase)
            outText.text += ((int)value).ToString();

        else if (Mathf.Abs(value) < 0.1f)
            outText.text += value.ToString("#,##0.##");

        else
            outText.text += value.ToString("#,##0.#");
    }

    private void SetEfficiency(int i)
    {
        TextMeshProUGUI setText = foodList[i].transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();

        if (addRange < 0)
            setText.color = Color.red;

        else
            setText.color = Color.blue;


        setText.text = (1 + addRange).ToString("#,##0.#%");
    }
}
