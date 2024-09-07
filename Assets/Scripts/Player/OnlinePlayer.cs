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

        // �̵� ���� üũ
        bool isWalk = Mathf.Abs(transform.position.x - beforePos.x) > error;
        animator.SetBool("isWalk", isWalk);

        // ���� �̵� ���� ���
        float deltaX = transform.position.x - beforePos.x;

        // �̵� ���⿡ ���� ��������Ʈ ���� ����
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