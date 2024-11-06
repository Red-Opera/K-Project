using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapabilityAgreement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainText;    // 남은 능력치 업그레이드 양을 확인할 수 있는 텍스트
    [SerializeField] private GameObject signType;           // 능력치 종류를 담는 오브젝트
    [SerializeField] private AudioClip signSound;           // 사인 소리
    [SerializeField] private HpLevelManager hpLevelManager; // 체력 레벨 관련 컴포넌트
    [SerializeField] private AbilityTempState AbilityStateData; // 어빌리티 레벨 값 저장 데이터

    [SerializeField] private SerializableDictionary<string, string> stateKoreaToEng;        // 추가 스탯의 한국어를 영어로 바꿔주는 배열
    [SerializeField] private List<AddAbilityKey> addAbilityKey;                             // 추가 능력치를 저장하는 리스트 Key
    [SerializeField] private List<AddAbilityValue> addAbilityValue;                         // 추가 능력치를 저장하는 리스트 Value
    private List<KeyValuePair<List<string>, List<double>>> addAbilitys;                     // 추가 능력치를 저장하는 리스트

    private List<TextMeshProUGUI> abilityText;
    private List<ParticleSystem> buyEffect;                     // 구매 효과
    private List<Ani2DRun> signEffect;                          // 사인하는 효과를 내는 스크립트

    private AudioSource audioSource;                            // 오디오 소스

    private int ableRemain;
    private const int MaxAbilityValue = 15;

    [System.Serializable] private class AddAbilityKey { [SerializeField] public List<string> key; }
    [System.Serializable] private class AddAbilityValue { [SerializeField] public List<double> value; }

    public void Start()
    {
        Debug.Assert(remainText != null, "남은 비용 텍스트 UI가 없습니다.");
        Debug.Assert(signType != null, "능력치 종류를 담는 오브젝트가 없습니다.");
        Debug.Assert(hpLevelManager != null, "체력, 레벨바 관련 컴포넌트가 존재하지 않습니다.");
        
        abilityText = new List<TextMeshProUGUI>();
        buyEffect = new List<ParticleSystem>();
        signEffect = new List<Ani2DRun>();

        audioSource = GetComponent<AudioSource>();

        foreach (Transform type in signType.transform)
        {
            // 각각의 능력 업그레이트 횟수가 적혀있는 텍스트를 가져옴
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

        // 총 업그레이드 한 양을 구함
        int totalUpgrade = 0;
        for (int i = 0; i < abilityText.Count; i++)
            totalUpgrade += int.Parse(abilityText[i].text);

        // 더 올릴 수 있는 능력치량을 구한 후 (Level * 3 - 업그레이드 수) 텍스트에 반영
        ableRemain = GameManager.info.allPlayerState.level * 3 - totalUpgrade;
        remainText.text = ableRemain.ToString();

        addAbilitys = new List<KeyValuePair<List<string>, List<double>>>();

        // 인스펙터에 입력한 스탯 업시 증가하는 값을 KeyPair에 저장함
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
        int currentUpgrade = int.Parse(upgradeTarget.text); // ability의 최댓값 지정

        if(currentUpgrade >= MaxAbilityValue){
            Debug.Log("최대 능력치에 도달했습니다.");
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
        Debug.Assert(ability != null, "추가 효과를 담는 오브젝트가 없습니다.");

        for (int i = 0; i < addAbilitys[index].Key.Count; i++)
        {
            // 추가되는 값 영역
            Transform addTarget = ability.GetChild(i).GetChild(1);

            // 선택한 능력치 주문서에서 사용되는 능력치 종류와 가치를 가져옴
            List<string> key = addAbilitys[index].Key;
            List<double> value = addAbilitys[index].Value;

            // 기존 값을 가져옴
            TextMeshProUGUI defaultValue = addTarget.GetComponent<TextMeshProUGUI>();

            // 최종 값을 구함
            double resultValue = double.Parse(defaultValue.text) + value[i];

            if (Math.Abs(resultValue) < 0.1)
                defaultValue.text = "+" + resultValue.ToString("#,##0.##");

            else
                defaultValue.text = "+" + resultValue.ToString("#,##0.#");

            // 추가 능력치를 반영함
            GameManager.info.SetStatState(stateKoreaToEng[key[i]], resultValue);
            int currentStatLevel = (int)(resultValue/value[i]);
            UpdateStaLevel(currentStatLevel, key[i]);
        }

        // 체력바 업데이트
        GameManager.info.allPlayerState.currentHp = GameManager.info.allPlayerState.maxHP;
        hpLevelManager.GetState(GameManager.info.allPlayerState);
    }

    void UpdateStaLevel(int CSLevel, string key){
        switch (key){
            case "공격력":
                GameManager.info.abilityState.Anger = CSLevel;
                break;

            case "이동 속도":
            case "공격 속도":
                GameManager.info.abilityState.Haste = CSLevel;
                break;
            case "방어력":
                GameManager.info.abilityState.Patience = CSLevel;
                break;

            case "치명타 확률":
            case "회피 확률":
                GameManager.info.abilityState.Mystery = CSLevel;
                break;

            case "최대 체력":
                GameManager.info.abilityState.Greed = CSLevel;
                break;

            case "강인함":
                GameManager.info.abilityState.Craving = CSLevel;
                break;

            default:
                break;
            }
    }
}