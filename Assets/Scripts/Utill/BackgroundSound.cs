using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioClip startClip;               // ���� �κ� �������
    [SerializeField] private AudioClip whileClip;               // �ݺ� �κ� �������
    [SerializeField] private AudioClip startBossClip;           // ������ ���� �κ� ��� ����
    [SerializeField] private AudioClip whileBossClip;           // ������ �ݺ� �κ� ��� ����

    private AudioSource audioSource;                // ����� �ҽ�

    private static AudioSource staticAudioSource;   // ����� �ҽ� Static ����
    private static AudioClip staticStartClip;       // ���� �κ� ������� Static ����
    private static AudioClip staticStartBossClip;   // ������ ���� �κ� ��� ���� Static ����
    private static bool isBossStage = false;        // ���� ���� ���������� ��� 
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource != null, "������� �����ϴ�.");

        staticAudioSource = audioSource;

        if (startBossClip != null)
            staticStartBossClip = startBossClip;

        // �� ó���� ��������� ó�� �κ��� ����ְ� �ݺ� ������ �����
        if (startClip != null && !isBossStage)
        {
            audioSource.clip = startClip;
            audioSource.Play();
        }

        else if (startBossClip != null && isBossStage)
        {
            audioSource.clip = startBossClip;
            audioSource.Play();
        }
    }

    public void Update()
    {
        if (!audioSource.isPlaying && !isBossStage)
        {
            audioSource.clip = whileClip;
            audioSource.Play();
        }

        else if (!audioSource.isPlaying && isBossStage)
        {
            audioSource.clip = whileBossClip;
            audioSource.Play();
        }
    }

    public static void StartBossClip()
    {
        if (isBossStage)
            return;

        isBossStage = true;

        if (staticStartBossClip != null)
            staticAudioSource.clip = staticStartBossClip;
        staticAudioSource.Play();
    }

    public static void NoBossClip()
    {
        if (!isBossStage)
            return;

        isBossStage = false;

        if (staticStartBossClip != null)
            staticAudioSource.clip = staticStartClip;
        staticAudioSource.Play();
    }
}