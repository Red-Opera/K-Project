using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PMove : MonoBehaviour
{
    Rigidbody2D rigid;
    CapsuleCollider2D PlayerCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public State playerState;
    public bool isJumping;
    public bool isAttack = false;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Start(){
        playerState = GameManager.info.playerState;
    }


    void Update()
    {
        Move();
        Jump();
        Dash();
        if(playerState.currentHp <= 0 &&gameObject.layer != 12){
            Die();
        }
    }

    void Move(){
        float Horiz = Input.GetAxisRaw("Horizontal");
        if(Horiz != 0){
            rigid.velocity = new Vector2(Horiz * playerState.moveSpeed, rigid.velocity.y);
            if(rigid.velocity.x>0){
                spriteRenderer.flipX = false;
            }else if(rigid.velocity.x<0){
                spriteRenderer.flipX = true;
            }
            anim.SetBool("isWalk",true);
        }
        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity =new Vector2(0, rigid.velocity.y);
            anim.SetBool("isWalk", false);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            playerState.moveSpeed *=1.5f;
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            if(rigid.velocity.x != 0){
                anim.SetBool("isRun",true);
            }else{
                anim.SetBool("isRun",false);
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            playerState.moveSpeed /= 1.5f;
            anim.SetBool("isRun",false);
        }
    }
    void Jump(){
        float ColliderSizeX = PlayerCollider.size.x/2;
        float ColliderSizeY = PlayerCollider.size.y/2 -PlayerCollider.offset.y;

        if(Input.GetButtonDown("Jump")){
            rigid.velocity = new Vector2(rigid.velocity.x, playerState.jumpPower);
            playerState.jumpCount ++;
            isJumping = true;
            Invoke("OnJump", 0.03f);
        }
        Vector2 PlayerPos = new Vector2(transform.position.x, transform.position.y);
        var GroundHit = Physics2D.OverlapArea(PlayerPos - new Vector2(ColliderSizeX,ColliderSizeY), PlayerPos - new Vector2(-ColliderSizeX,ColliderSizeY),LayerMask.GetMask("Platform","DamagedObject"));
        if(GroundHit != null && isJumping == false){
            anim.SetBool("isJump",false);
            if(playerState.jumpCount > 0){
                playerState.jumpCount = 0;
            }
        }
        
    }
    void OnJump(){
        isJumping = false;
        anim.SetBool("isJump",true);
    }

    void Cooldown(){
        isAttack = false;
    }

    void Dash(){
        if(Input.GetMouseButtonDown(1) && playerState.dashBarCount > 0){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dashDir = mousePos - rigid.position;
            rigid.velocity += dashDir.normalized* 8;
            playerState.dashBarCount--;

            if(dashDir.x >0){
                spriteRenderer.flipX = false;
            }
            else if(dashDir.x <0){
                spriteRenderer.flipX = true;
            }
 
            gameObject.layer = 9;
            Invoke("OnJump", 0.03f);
            Invoke("DashEnd",0.8f);
        }
    }
    void DashEnd(){
        gameObject.layer = 8;
    }
    void Die(){
        anim.SetTrigger("Die");
        gameObject.layer = 12;
    }
}
