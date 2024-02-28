using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D PlayerCollider;
    Animator anim;

    public State playerState;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
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
            anim.SetBool("isWalk",true);
            if(rigid.velocity.x > 0){
                spriteRenderer.flipX =false;
            }else if(rigid.velocity.x <0){
                spriteRenderer.flipX =true;
            }
        }else{
            anim.SetBool("isWalk",false);
        }

        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            playerState.moveSpeed *= 1.5f;
            anim.SetBool("isRun",true);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            playerState.moveSpeed /= 1.5f;
            anim.SetBool("isRun",false);
        }
    }

    void Jump(){
        float ColliderSizeX = PlayerCollider.size.x/2;
        float ColliderSizeY = PlayerCollider.size.y/2 - PlayerCollider.offset.y;
        
        if(Input.GetButtonDown("Jump") && playerState.jumpCount <playerState.maxJump){
            rigid.velocity = new Vector2(rigid.velocity.x, playerState.jumpPower);
            playerState.jumpCount++;
            Invoke("OnJumpAnim",0.03f);
        }
        
        Vector2 PlayerPos = new Vector2(transform.position.x, transform.position.y);
        var GroundHit = Physics2D.OverlapArea(PlayerPos - new Vector2(ColliderSizeX,ColliderSizeY),PlayerPos - new Vector2(-ColliderSizeX,ColliderSizeY),LayerMask.GetMask("Platform"));
        if(GroundHit != null){
            anim.SetBool("isJump",false);
            if(playerState.jumpCount > 0){
                playerState.jumpCount =0;
            }
        }
    }
    void OnJumpAnim(){
        anim.SetBool("isJump",true);
        Debug.Log("setBool");
    }
    //부를때 움직이지 않게
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
