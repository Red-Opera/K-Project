using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapabilityAgreement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainText;    // ���� �ɷ�ġ ���׷��̵� ���� Ȯ���� �� �ִ� �ؽ�Ʈ
    [SerializeField] private GameObject signType;           // �ɷ�ġ ������ ��� ������Ʈ
    [SerializeField] private AudioClip signSound;           // ���� �Ҹ�
    [SerializeField] private HpLevelManager hpLevelManager; // ü�� ���� ���� ������Ʈ
    [SerializeField] private AbilityTempState AbilityStateData; // �����Ƽ ���� �� ���� ������

    [SerializeField] private SerializableDictionary<string, string> stateKoreaToEng;        // �߰� ������ �ѱ�� ����� �ٲ��ִ� �迭
    [SerializeField] private List<AddAbilityKey> addAbilityKey;                             // �߰� �ɷ�ġ�� �����ϴ� ����Ʈ Key
    [SerializeField] private List<AddAbilityValue> addAbilityValue;                         // �߰� �ɷ�ġ�� �����ϴ� ����Ʈ Value
    private List<KeyValuePair<List<string>, List<double>>> addAbilitys;                     // �߰� �ɷ�ġ�� �����ϴ� ����Ʈ

    private List<TextMeshProUGUI> abilityText;
    private List<ParticleSystem> buyEffect;                     // ���� ȿ��
    private List<Ani2DRun> signEffect;                          // �����ϴ� ȿ���� ���� ��ũ��Ʈ

    private AudioSource audioSource;                            // ����� �ҽ�

    private int ableRemain;
    private const int MaxAbilityValue = 15;

    [System.Serializable] private class AddAbilityKey { [SerializeField] public List<string> key; }
    [System.Serializable] private class AddAbilityValue { [SerializeField] public List<double> value; }

    public void Start()
    {
        Debug.Assert(remainText != null, "���� ��� �ؽ�Ʈ UI�� �����ϴ�.");
        Debug.Assert(signType != null, "�ɷ�ġ ������ ��� ������Ʈ�� �����ϴ�.");
        Debug.Assert(hpLevelManager != null, "ü��, ������ ���� ������Ʈ�� �������� �ʽ��ϴ�.");
        
        abilityText = new List<TextMeshProUGUI>();
        buyEffect = new List<ParticleSystem>();
        signEffect = new List<Ani2DRun>();

        audioSource = GetComponent<AudioSource>();

        foreach (Transform type in signType.transform)
        {
            // ������ �ɷ� ���׷���Ʈ Ƚ���� �����ִ� �ؽ�Ʈ�� ������
            for (int i = 0; i < type.childCount; i++)
            {
                if (type.GetChild(i).name == "SignCount")
                    abilityText.Add(type.GetChild(i).GetComponent<TextMeshProUGUI>());

                if (type.GetChild(i).name == "FeatherParticle")
                    buyEffect.Add(type.GetChild(i).GetComponent<ParticleSystem>());

                if (type.GetChild(i).name == "Sign")
                    signEffect.Add(type.GetChild(i).GetComponent<Ani2DRun>());
            }
        }

        // �� ���׷��̵� �� ���� ����
        int totalUpgrade = 0;
        for (int i = 0; i < abilityText.Count; i++)
            totalUpgrade += int.Parse(abilityText[i].text);

        // �� �ø� �� �ִ� �ɷ�ġ���� ���� �� (Level * 3 - ���׷��̵� ��) �ؽ�Ʈ�� �ݿ�
        ableRemain = GameManager.info.allPlayerState.level * 3 - totalUpgrade;
        remainText.text = ableRemain.ToString();

        addAbilitys = new List<KeyValuePair<List<string>, List<double>>>();

        // �ν����Ϳ� �Է��� ���� ���� �����ϴ� ���� KeyPair�� ������
        for (int i = 0; i < addAbilityKey.Count; i++)
        {
            KeyValuePair<List<string>, List<double>> keyValue = new KeyValuePair<List<string>, List<double>>(addAbilityKey[i].key, addAbilityValue[i].value);

            addAbilitys.Add(keyValue);
        }
    }
    public void Update()
    {
        
    }

    public void AbilitySign(int index)
    {
        int remain = int.Parse(remainText.text);

        if (remain <= 0)
            return;

        TextMeshProUGUI upgradeTarget = abilityText[index];
        int currentUpgrade = int.Parse(upgradeTarget.text); // ability�� �ִ� ����

        if(currentUpgrade >= MaxAbilityValue){
            Debug.Log("�ִ� �ɷ�ġ�� �����߽��ϴ�.");
            return;
        }
        upgradeTarget.text = (int.Parse(upgradeTarget.text) + 1).ToString();
        
        ableRemain--;
        remainText.text = ableRemain.ToString();

        buyEffect[index].Play();
        audioSource.PlayOneShot(signSound);

        StateRefresh(index);

        StartCoroutine(signEffect[index].Play(false));
    }

    private void StateRefresh(int index)
    {
        Transform sign = signType.transform.GetChild(index);
        Transform ability = sign.Find("AddAbility");
        Debug.Assert(ability != null, "�߰� ȿ���� ��� ������Ʈ�� �����ϴ�.");

        for (int i = 0; i < addAbilitys[index].Key.Count; i++)
        {
            // �߰��Ǵ� �� ����
            Transform addTarget = ability.GetChild(i).GetChild(1);

            // ������ �ɷ�ġ �ֹ������� ���Ǵ� �ɷ�ġ ������ ��ġ�� ������
            List<string> key = addAbilitys[index].Key;
            List<double> value = addAbilitys[index].Value;

            // ���� ���� ������
            TextMeshProUGUI defaultValue = addTarget.GetComponent<TextMeshProUGUI>();

            // ���� ���� ����
            double resultValue = double.Parse(defaultValue.text) + value[i];

            if (Math.Abs(resultValue) < 0.1)
                defaultValue.text = "+" + resultValue.ToString("#,##0.##");

            else
                defaultValue.text = "+" + resultValue.ToString("#,##0.#");

            // �߰� �ɷ�ġ�� �ݿ���
            GameManager.info.SetStatState(stateKoreaToEng[key[i]], resultValue);
            int currentStatLevel = (int)(resultValue/value[i]);
            UpdateStaLevel(currentStatLevel, key[i]);
        }

        // ü�¹� ������Ʈ
        GameManager.info.allPlayerState.currentHp = GameManager.info.allPlayerState.maxHP;
        hpLevelManager.GetState(GameManager.info.allPlayerState);
    }

    void UpdateStaLevel(int CSLevel, string key){
        switch (key){
            case "���ݷ�":
                GameManager.info.abilityState.Anger = CSLevel;
                break;

            case "�̵� �ӵ�":
            case "���� �ӵ�":
                GameManager.info.abilityState.Haste = CSLevel;
                break;
            case "����":
                GameManager.info.abilityState.Patience = CSLevel;
                break;

            case "ġ��Ÿ Ȯ��":
            case "ȸ�� Ȯ��":
                GameManager.info.abilityState.Mystery = CSLevel;
                break;

            case "�ִ� ü��":
                GameManager.info.abilityState.Greed = CSLevel;
                break;

            case "������":
                GameManager.info.abilityState.Craving = CSLevel;
                break;

            default:
                break;
            }
    }
}