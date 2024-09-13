using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    public string playerName = "";

    private Animator animator;              // �ٸ� ĳ������ Animation
    private SpriteRenderer spriteRenderer;  // �ٸ� ĳ������ SpriteRenderer

    private Vector3 beforePos;              // ���� ��ġ

    private int processServerFrame = 0;     // ó���� ������

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        beforePos = transform.position;
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
    }

    private void Update()
    {
        if (processServerFrame == MultiPlay.currentServerFrame)
            return;

        // Sprite�� ������ ����
        if (MultiPlay.currentClientSpriteFlip.IndexOf(playerName) != -1)
            spriteRenderer.flipX = true;

        else
            spriteRenderer.flipX = false;

        // �ٸ� Ŭ���̾�Ʈ�� �ִϸ��̼��� ����� ���
        if (MultiPlay.currentClientAnimationName.ContainsKey(playerName))
            SetPlayerAnimation();

        beforePos = transform.position;
        processServerFrame = MultiPlay.currentServerFrame;
    }

    private void SetPlayerAnimation()
    {
        AnimatorClientData animationData = MultiPlay.currentClientAnimationName[playerName];

        animator.Play(int.Parse(animationData.animationName), 0, animationData.animationNormalizedTime);
        animator.StopPlayback();
    }
}