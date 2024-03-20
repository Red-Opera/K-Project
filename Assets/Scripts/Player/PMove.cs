using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
    public GameObject nAtkObject;
    public GameObject mAtkObject;

    public State playerState;
    public bool isJumping;
    public bool isAttack = false;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //공격 시 생성할 오브젝트 초기값 설정
        nAtkObject = Resources.Load<GameObject>("Prefab/Character/PhysicAttack");
        mAtkObject = Resources.Load<GameObject>("Prefab/Character/MagicSpear");
    }


    void Update()
    {
        Move();
        Jump();
        if(Input.GetMouseButton(0) && isAttack == false){
            Attack();
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

    void Attack(){
        isAttack = true;
        int Dir = 1;

        //flipX의 값에 따라 공격 생성 방향 지정
        if(spriteRenderer.flipX){
            Dir = -1;
        }else{
            Dir = 1;
        }

        //공격 시 X축 이동 속도 감소
        rigid.velocity = new Vector2(rigid.velocity.x *.5f, rigid.velocity.y);

        //방향을 반영하여 공격 오브젝트 생성, 생성된 오브젝트가 플레이어를 따라가도록 설정
        GameObject nAtk = Instantiate(nAtkObject, rigid.position + new Vector2(0.8f * Dir,0), quaternion.identity);
        nAtk.transform.SetParent(rigid.transform);

        //공격 속도로 지정된 시간이후로 재공격 가능하도록 함수 호출
        Invoke("Cooldown", 0.8f);
        anim.SetTrigger("nAttack");
    }

    void MagicAttack(){
        isAttack =true;
        int Dir = 1;

        if(spriteRenderer.flipX){
            Dir = -1;
        }else{
            Dir = 1;
        }

        rigid.velocity = new Vector2(rigid.velocity.x *0.2f, rigid.velocity.y);

        GameObject mAtk = Instantiate(mAtkObject, rigid.position + new Vector2(0.8f*Dir,0),quaternion.identity);
        anim.SetTrigger("mAttack");
        Invoke("Cooldown",0.75f);
    }

    void Cooldown(){
        isAttack = false;
    }
}
