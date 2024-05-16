using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioClip startClip;               // 시작 부분 배경음악
    [SerializeField] private AudioClip whileClip;               // 반복 부분 배경음악
    [SerializeField] private AudioClip startBossClip;           // 보스전 시작 부분 배경 음악
    [SerializeField] private AudioClip whileBossClip;           // 보스전 반복 부분 배경 음악

    private AudioSource audioSource;                // 오디오 소스

    private static AudioSource staticAudioSource;   // 오디오 소스 Static 버전
    private static AudioClip staticStartClip;       // 시작 부분 배경음악 Static 버전
    private static AudioClip staticStartBossClip;   // 보스전 시작 부분 배경 음악 Static 버전
    private static bool isBossStage = false;        // 현재 보스 스테이지인 경우 
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource != null, "오디오가 없습니다.");

        staticAudioSource = audioSource;

        if (startBossClip != null)
            staticStartBossClip = startBossClip;

        // 맨 처음에 배경음악의 처음 부분을 들려주고 반복 음악을 재생함
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