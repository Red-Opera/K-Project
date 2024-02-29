using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapabilityAgreement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainText;    // 남은 능력치 업그레이드 양을 확인할 수 있는 텍스트
    [SerializeField] private GameObject signType;           // 능력치 종류를 담는 오브젝트
    [SerializeField] private AudioClip signSound;           // 사인 소리

    private List<TextMeshProUGUI> abilityText;
    private List<ParticleSystem> buyEffect;      // 구매 효과
    private List<Ani2DRun> signEffect;           // 사인하는 효과를 내는 스크립트

    private AudioSource audioSource;             // 오디오 소스

    private int ableRemain;

    public void Start()
    {
        Debug.Assert(remainText != null, "남은 비용 텍스트 UI가 없습니다.");
        Debug.Assert(signType != null, "능력치 종류를 담는 오브젝트가 없습니다.");
        
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
        upgradeTarget.text = (int.Parse(upgradeTarget.text) + 1).ToString();
        
        ableRemain--;
        remainText.text = ableRemain.ToString();

        buyEffect[index].Play();
        audioSource.PlayOneShot(signSound);

        StartCoroutine(signEffect[index].Play(false));
    }
}
