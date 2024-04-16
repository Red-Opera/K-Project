using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioClip startClip;               // 시작 부분 배경음악
    [SerializeField] private AudioClip whileClip;               // 반복 부분 배경음악

    private AudioSource audioSource;           // 오디오 소스
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource != null, "오디오가 없습니다.");

        // 맨 처음에 배경음악의 처음 부분을 들려주고 반복 음악을 재생함
        audioSource.clip = startClip;
        audioSource.Play();
    }

    public void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = whileClip;
            audioSource.Play();
        }
    }
}