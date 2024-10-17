using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    public string playerName = "";

    private Animator animator;              // 다른 캐릭터의 Animation
    private SpriteRenderer spriteRenderer;  // 다른 캐릭터의 SpriteRenderer

    private Vector3 beforePos;              // 이전 위치

    private int processServerFrame = 0;     // 처리한 프레임

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

        // Sprite의 방향을 정함
        if (MultiPlay.currentClientSpriteFlip.IndexOf(playerName) != -1)
            spriteRenderer.flipX = true;

        else
            spriteRenderer.flipX = false;

        // 다른 클라이언트의 애니메이션이 변경된 경우
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