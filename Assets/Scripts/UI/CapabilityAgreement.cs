using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapabilityAgreement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainText;    // ���� �ɷ�ġ ���׷��̵� ���� Ȯ���� �� �ִ� �ؽ�Ʈ
    [SerializeField] private GameObject signType;           // �ɷ�ġ ������ ��� ������Ʈ
    [SerializeField] private AudioClip signSound;           // ���� �Ҹ�

    private List<TextMeshProUGUI> abilityText;
    private List<ParticleSystem> buyEffect;      // ���� ȿ��
    private List<Ani2DRun> signEffect;           // �����ϴ� ȿ���� ���� ��ũ��Ʈ

    private AudioSource audioSource;             // ����� �ҽ�

    private int ableRemain;

    public void Start()
    {
        Debug.Assert(remainText != null, "���� ��� �ؽ�Ʈ UI�� �����ϴ�.");
        Debug.Assert(signType != null, "�ɷ�ġ ������ ��� ������Ʈ�� �����ϴ�.");
        
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
