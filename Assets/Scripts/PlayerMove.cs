using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D PlayerCollider;

    public State playerState;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move(){
        float Horiz = Input.GetAxisRaw("Horizontal");
        if(Horiz != 0){
            rigid.velocity = new Vector2(Horiz * playerState.moveSpeed, rigid.velocity.y);
        }
        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            playerState.moveSpeed *= 1.5f;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            playerState.moveSpeed /= 1.5f;
        }

        if(rigid.velocity.x > 0){
            spriteRenderer.flipX = false;
        }
        else if(rigid.velocity.x <0){
            spriteRenderer.flipX = true;
        }
    }

    void Jump(){
        float ColliderSizeX = PlayerCollider.size.x/2;
        float ColliderSizeY = PlayerCollider.size.y/2 - PlayerCollider.offset.y;
        if(Input.GetButtonDown("Jump") && playerState.jumpCount <playerState.maxJump){
            rigid.velocity = new Vector2(rigid.velocity.x, playerState.jumpPower);
            playerState.jumpCount++;
        }
        
        Vector2 PlayerPos = new Vector2(transform.position.x, transform.position.y);
        var GroundHit = Physics2D.OverlapArea(PlayerPos - new Vector2(ColliderSizeX,ColliderSizeY),PlayerPos - new Vector2(-ColliderSizeX,ColliderSizeY),LayerMask.GetMask("Platform"));
        if(GroundHit != null && playerState.jumpCount > 0){
            playerState.jumpCount =0;
        }
    }
}
