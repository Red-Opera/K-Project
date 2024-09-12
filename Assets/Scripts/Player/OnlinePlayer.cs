using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayer : MonoBehaviour
{
    public string playerName = "";

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 beforePos;
    private readonly float error = 0.00005f;
    private bool isFacingRight = true;

    private int processServerFrame = 0;
    private int currentProcessFrame = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        beforePos = transform.position;
    }

    private void Update()
    {
        if (processServerFrame == MultiPlay.currentServerFrame && !IsThisClientUpdate())
            return;

        // 이동 여부 체크
        bool isWalk = Mathf.Abs(transform.position.x - beforePos.x) > error;
        bool isJump = Mathf.Abs(transform.position.y - beforePos.y) > error;

        //if(!isWalk)
        //    Debug.Log(processServerFrame.ToString() + "::" + MultiPlay.currentServerFrame.ToString());
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isJump", isJump);

        Debug.LogWarning(processServerFrame + "::" + MultiPlay.currentServerFrame);

        if (MultiPlay.currentClientAttack.IndexOf(playerName) != -1)
            animator.SetTrigger("mAttack");

        // Sprite의 방향을 정함
        if (MultiPlay.currentClientSpriteFlip.IndexOf(playerName) != -1)
            spriteRenderer.flipX = true;

        else
            spriteRenderer.flipX = false;

        beforePos = transform.position;
        processServerFrame = MultiPlay.currentServerFrame;
    }

    private bool IsThisClientUpdate()
    {
        if (MultiPlay.clients[playerName].frame == currentProcessFrame)
            return false;

        currentProcessFrame = MultiPlay.clients[playerName].frame;

        return true;
    }
}