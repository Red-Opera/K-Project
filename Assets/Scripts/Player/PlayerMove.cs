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
        //공격 중이거나 대쉬 중일때 기동하지 않음, 공격 시 조작 여부는 논의할 필요가 있지만 대쉬 중일때 이 코드가 기동하지 않는 것은 유지가 필요해 보임
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

        //flipX의 값에 따라 일정 속도 만큼 대쉬, 마우스 방향으로 대쉬할지 논의가 필요함
        //현재 대쉬 애니메이션을 점프 애니메이션과 동일하게 설정하였는데 이에 대해 논의가 필요함
        if(Input.GetMouseButtonDown(1) && isDash == false){
            if(spriteRenderer.flipX == false){
                rigid.AddForce(new Vector2(8,0.5f), ForceMode2D.Impulse);
            }
            else{
                rigid.AddForce(new Vector2(-8,0.5f), ForceMode2D.Impulse);
            }
            isDash = true;
            gameObject.layer = 9;
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
        
        //점프, 점프 애니메이션 점프 애니메이션을 0.03초 뒤에 출력하는 것으로 점프하자마자 jumpCount가 초기화 되는 것을 막음
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
        //모션 캔슬을 방지하기 위해 공격 자체가 cooldown을 줌 ,공격시 veloxit.x값을 제한 했지만 구현방식에 대해선 의논이 필요함
        isAtk = true;
        int Dir = 1;
        
        if(spriteRenderer.flipX == true)
            Dir =- 1;
        else
            Dir = 1;

        rigid.velocity = new Vector2(rigid.velocity.x * 0.5f, rigid.velocity.y);
        
        GameObject nAtk = Instantiate(nAtkObj, rigid.position+ new Vector2(0.5f * Dir,0),quaternion.identity);
        nAtk.transform.SetParent(rigid.transform);
        NormalAttack nAtkScript = nAtk.GetComponent<NormalAttack>();
        
        if(nAtkScript != null){
            nAtkScript.setDamage(playerState.damage);
        }

        Invoke("Cooldown",0.8f);
        anim.SetTrigger("nAttack");
    }

    void MagicAttack(){
        isAtk = true;
        int Dir;
        if(spriteRenderer.flipX == true){
            Dir = -1;
        }
        else{
            Dir = 1;
        }
        GameObject mAtk = Instantiate(mAtkObj, rigid.position + new Vector2(1 * Dir,0), quaternion.identity);
        MagicAttack mAtkScript = mAtk.GetComponent<MagicAttack>();

        if(mAtkScript != null){
            mAtkScript.setDamage(playerState.damage);
        }
        anim.SetTrigger("mAttack");
        Invoke("Cooldown", 0.75f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 6){
            Damaged(5);
            rigid.AddForce(new Vector2(rigid.velocity.x, 3),ForceMode2D.Impulse);
            anim.SetTrigger("damaged");
            Debug.Log(playerState.currentHp);
        }
    }

    public void Damaged(int Dmg){
        playerState.currentHp -= Dmg;
    }

    void Cooldown(){
        isAtk =false;
        isDash = false;
        //마우스 방향을 대시 할 시 구현
        if(gameObject.layer == 9){
            gameObject.layer =8;
        }
    }
}
