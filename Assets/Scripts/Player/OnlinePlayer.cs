using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 beforePos;
    private readonly float error = 0.01f;
    private bool isFacingRight = true;

    private int processServerFrame = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        beforePos = transform.position;
    }

    private void Update()
    {
        if (processServerFrame == MultiPlay.currentServerFrame)
            return;

        // 이동 여부 체크
        bool isWalk = Mathf.Abs(transform.position.x - beforePos.x) > error;
        animator.SetBool("isWalk", isWalk);

        // 현재 이동 방향 계산
        float deltaX = transform.position.x - beforePos.x;

        // 이동 방향에 따라 스프라이트 방향 설정
        if (deltaX > error && !isFacingRight)
            Flip();

        else if (deltaX < -error && isFacingRight)
            Flip();

        beforePos = transform.position;
        processServerFrame = MultiPlay.currentServerFrame;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}