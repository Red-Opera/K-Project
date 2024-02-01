using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D PlayerCollider;
    Animator anim;

    public State playerState;
    public GameObject nAtkObj;
    public GameObject mAtkObj;
    public bool isAtk = false;
    public bool isDash = false;
    public bool isJumping = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        playerState.currentHp = playerState.maxHP;
    }

    void Update()
    {
        Move();
        Jump();
        if(Input.GetMouseButtonDown(0)&& isAtk == false){
        //if(Input.GetKeyDown(KeyCode.Z) && isAtk == false){
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.LeftControl) && isAtk ==false){
            MagicAttack();
        }
    }

    void Move(){
        float Horiz = Input.GetAxisRaw("Horizontal");
        if(Horiz != 0 && isAtk == false && isDash == false){
            rigid.velocity = new Vector2(Horiz * playerState.moveSpeed, rigid.velocity.y);
            anim.SetBool("isWalk",true);
            if(rigid.velocity.x > 0){
                spriteRenderer.flipX =false;
            }else if(rigid.velocity.x <0){
                spriteRenderer.flipX =true;
            }
        }

        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            anim.SetBool("isWalk",false);
        }

        
        if(Input.GetMouseButtonDown(1) && isDash == false){
            if(spriteRenderer.flipX == false){
                rigid.AddForce(new Vector2(8,0.5f), ForceMode2D.Impulse);
            }
            else{
                Debug.Log("??");
                rigid.AddForce(new Vector2(-8,0.5f), ForceMode2D.Impulse);
            }
            isDash = true;
            anim.SetBool("isJump",true);
            Invoke("Cooldown", 0.5f);
        }
        

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            playerState.moveSpeed *= 1.5f;
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
        float ColliderSizeY = PlayerCollider.size.y/2 - PlayerCollider.offset.y;
        
        if(Input.GetButtonDown("Jump") && playerState.jumpCount <playerState.maxJump){
            rigid.velocity = new Vector2(rigid.velocity.x, playerState.jumpPower);
            playerState.jumpCount++;
            isJumping = true;
            Invoke("OnJumpAnim",0.03f);
        }
        
        Vector2 PlayerPos = new Vector2(transform.position.x, transform.position.y);
        var GroundHit = Physics2D.OverlapArea(PlayerPos - new Vector2(ColliderSizeX,ColliderSizeY),PlayerPos - new Vector2(-ColliderSizeX,ColliderSizeY),LayerMask.GetMask("Platform","DamagedObject"));
        if(GroundHit != null && isJumping == false && isDash == false){
            anim.SetBool("isJump",false);
            if(playerState.jumpCount > 0){
                playerState.jumpCount =0;
            }
        }
    }
    void OnJumpAnim(){
        isJumping = false;
        anim.SetBool("isJump",true);
    }

    void Attack(){
        isAtk = true;
        Invoke("Cooldown",0.8f);
        rigid.velocity = new Vector2(rigid.velocity.x * 0.5f, rigid.velocity.y);
        int atkDir;
        if(spriteRenderer.flipX == true){
            atkDir =-1;
        }
        else{
            atkDir =1;
        }
        GameObject nAtkOb = Instantiate(nAtkObj, rigid.position+ new Vector2(0.5f*atkDir,0),quaternion.identity);
        nAtkOb.transform.SetParent(rigid.transform);
        anim.SetTrigger("nAttack");
    }

    void MagicAttack(){
        isAtk = true;
        Instantiate(mAtkObj, rigid.position + new Vector2(1,0), quaternion.identity);
        anim.SetTrigger("mAttack");
        Invoke("Cooldown", 0.75f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 6){
            Damaged(5);
            rigid.AddForce(new Vector2(rigid.velocity.x, 3),ForceMode2D.Impulse);
            Debug.Log(playerState.currentHp);
        }
    }

    public void Damaged(int Dmg){
        playerState.currentHp -= Dmg;
    }

    void Cooldown(){
        isAtk =false;
        isDash = false;
    }
}
