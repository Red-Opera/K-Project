using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BossController : MonoBehaviour
{
    public BossMonster Boss;
    Animator anim;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    public HpLevelManager BossHp;
    public int moveSpeed = 0;
    bool findP = false;
    public bool isAtk = false;
    public bool isJump = false;
    public int attackType = 0;
    public int Damage;
    float WC = 0;
    public StageManager stageManager;
    // Start is called before the first frame update
    void Start()
    {
        Boss.InitSetting();
        Damage = Boss.boss.damage;
        spriteRenderer =GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        Boss.boss.bossState.currentHp = Boss.boss.bossState.maxHP;
        //hpLevelManager = FindObjectOfType<HpLevelManager>();
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
        rigid.velocity = new Vector2 (moveSpeed *Boss.boss.bossState.moveSpeed * Boss.boss.bossState.dashcoaf,rigid.velocity.y);
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
        isAtk =true;
        if(attackType ==0){
            Boss.Attack1();
            Invoke("AttackEnd",3);
        }else if(attackType ==1){
            Boss.Attack2();
            Invoke("AttackEnd",3);
        }else if(attackType ==2){
            Boss.SpecialAttack1();
            Invoke("IsJump", 0.6f);
            Invoke("AttackEnd",3);
        }
        else if(attackType ==3){
            Boss.SpecialAttack2();
            Invoke("AttackEnd",3);
        }
    }
    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == 3 && isJump == true){
            anim.SetBool("isJump",false);
            
        }
    }
    void IsJump(){
        isJump = true;
    }
    void AttackEnd(){
        isAtk =false;
        Boss.boss.bossState.dashcoaf = 1;
        attackType = Random.Range(0,Boss.boss.attackCount);
    }

    public void Damaged(int dmg, float WeakCoaf, float bleedCoaf){
        Boss.boss.bossState.currentHp -= dmg;
        if(bleedCoaf != 0){
            StartCoroutine(BleedingCoroutine(3, bleedCoaf));
        }
        BossHp.BossSliderReset();
        if(Boss.boss.bossState.currentHp <= 0){
            Boss.boss.bossState.damage = Damage;
            Die();
        }
        if(WeakCoaf != 0){
            WC = WeakCoaf;
            Boss.boss.bossState.damage -= (int)(Damage *WeakCoaf);
            Invoke("WeakEnd",5);
        }
    }
    void Die()
    {
        TimerManager timerManager = FindObjectOfType<TimerManager>();
        if (timerManager != null)
        {
            timerManager.BossDefeated();
        }
        if(stageManager != null){
            stageManager.rewardMap();
        }

        Destroy(gameObject);
    }

    void WeakEnd(){
        Boss.boss.bossState.damage += (int)(Damage*WC);
    }
    IEnumerator BleedingCoroutine(int bleedCount, float bleedCoaf){
        if(bleedCount > 0){
            Boss.boss.bossState.currentHp -= (int)(Boss.boss.bossState.maxHP * bleedCoaf);
            BossHp.BossSliderReset();
            if(Boss.boss.bossState.currentHp <= 0){
                Boss.boss.bossState.damage = Damage;
                Die();
                yield break;
            }
            yield return new WaitForSeconds(1);
            StartCoroutine(BleedingCoroutine(bleedCount-1, bleedCoaf));
        }
        yield break;
    }
}
