using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossMonster Boss;
    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    public MonsterState state;
    int moveSpeed = 0;
    bool findP = false;
    public bool isAtk = false;
    public int attackType = 0;
    // Start is called before the first frame update
    void Start()
    {
        Boss.InitSetting();
        spriteRenderer =GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        state = Resources.Load<MonsterState>("Scriptable/Boss/WareWolf");
        SetSpeed();
        AttackEnd();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
        DetectP();
        if(findP && !isAtk){
            Attack();
        }
    }

    void Idle(){
        rigid.velocity = new Vector2 (moveSpeed *state.moveSpeed * state.dashcoaf,rigid.velocity.y);
        if(rigid.velocity.x>0){
            spriteRenderer.flipX = false;
            anim.SetBool("isWalk",true);
            Boss.boss.dir = 1;
        }else if(rigid.velocity.x < 0){
            spriteRenderer.flipX = true;
            anim.SetBool("isWalk",true);
            Boss.boss.dir = -1;
        }else{
            anim.SetBool("isWalk",false);
        }
    }

    void SetSpeed(){
        if(findP){
            moveSpeed = Boss.boss.dir;
            Invoke("SetSpeed", 0.5f);
        }else{
            moveSpeed = Random.Range (-1, 2);
            Invoke("SetSpeed",1.5f);
        }
    }
    
    void DetectP(){
        Vector2 detectRange =  new Vector2(10*Boss.boss.dir,5);
        var detecP = Physics2D.OverlapArea(rigid.position + detectRange, rigid.position + new Vector2(0,-5), LayerMask.GetMask("Player"));
        if(detecP != null){
            findP = true;
        }else{
            findP = false;
        }
    }

    void Attack(){
        if(attackType ==0){
            Boss.Attack1();
            Invoke("IsAttack",Boss.boss.ainmterm);
            Invoke("AttackEnd",1);
        }else if(attackType ==1){
            Boss.Attack2();
            Invoke("IsAttack",Boss.boss.ainmterm);
            Invoke("AttackEnd",1);
        }else if(attackType ==2){
            Boss.SpecialAttack1();
            Invoke("IsAttack",Boss.boss.ainmterm);
        }
        else if(attackType ==3){
            if(Boss.boss.bossState.Stage == 1){
                AttackEnd();
            }else{
                Boss.SpecialAttack2();
                isAtk = true;
                Invoke("AttackEnd",1);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 3 && isAtk == true){
            anim.SetBool("isJump",false);
            isAtk = false;
            attackType = Random.Range(0, 3);
        }
    }
    void IsAttack(){
        isAtk = true;
    }
    void AttackEnd(){
        isAtk =false;
        attackType = Random.Range(0,3);
    }
}
