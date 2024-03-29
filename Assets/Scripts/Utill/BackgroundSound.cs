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
        audioSource.PlayOneShot(startClip);
        Invoke("BackgroundLoop", startClip.length - 0.75f);
    }

    private void BackgroundLoop()
    {
        audioSource.PlayOneShot(whileClip);
    }
}