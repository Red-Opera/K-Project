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
    // Start is called before the first frame update
    void Start()
    {
        Boss.InitSetting();
        spriteRenderer =GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        state = Resources.Load<MonsterState>("Scriptable/Boss/WareWolf");
        SetSpeed();
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
        rigid.velocity = new Vector2 (moveSpeed *state.moveSpeed,rigid.velocity.y);
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
        moveSpeed = Random.Range (-1, 2);
        Invoke("SetSpeed",1.5f);
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
        if(moveSpeed ==1){
            Boss.Attack1();
            isAtk = true;
            Invoke("AttackEnd",1);
            anim.SetTrigger("attack1");
        }else{
            Boss.Attack2();
            isAtk = true;
            Invoke("AttackEnd",1);
            anim.SetTrigger("attack2");
        }
    }
    void AttackEnd(){
        isAtk =false;
    }
}
