using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] private AudioClip startClip;               // ���� �κ� �������
    [SerializeField] private AudioClip whileClip;               // �ݺ� �κ� �������

    private AudioSource audioSource;           // ����� �ҽ�
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Assert(audioSource != null, "������� �����ϴ�.");

        // �� ó���� ��������� ó�� �κ��� ����ְ� �ݺ� ������ �����
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