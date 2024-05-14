using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKing : BossMonster
{
    Animator anim;
    public override void InitSetting()
    {
        boss.range = Resources.Load<GameObject>("Prefab/Boss/AttackRange");
        boss.bossState = Resources.Load<MonsterState>("Scriptable/Boss/SkeletonKing");
        boss.pos = new Vector3(2,0,0);
        boss.damage = boss.bossState.damage;
        boss.disapearTime = .8f;
        boss.attackCount = 4;
        anim = GetComponent<Animator>();
    }
    public override void Attack1()
    {
        boss.pos = new Vector3(0.7f,0,0);
        boss.range.transform.localScale = new Vector3(6.7f,4.4f,1);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir,Quaternion.identity );
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        StartCoroutine(DelayedTrigger("Attack", 0.5f));
    }

    public override void Attack2()
    {
        boss.pos = new Vector3(2.8f,0.1f,0);
        boss.range.transform.localScale = new Vector3(7.6f,3.6f,1);
        boss.bossState.dashcoaf =2;
        Invoke("ResetSpeed", 0.5f);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        StartCoroutine(DelayedTrigger("Dash", 0.5f));
    }

    public override void SpecialAttack1()
    {
        boss.pos = new Vector3(0,0,0);
        boss.range.transform.localScale = new Vector3(1.5f,2,1);
        Rigidbody2D rigid = GetComponent<Rigidbody2D> ();
        var findP = Physics2D.OverlapArea(rigid.position + new Vector2(10,5), rigid.position + new Vector2(-10,-5), LayerMask.GetMask("Player"));
        GameObject Atk = Instantiate(boss.range, findP.transform.position + boss.pos * boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        StartCoroutine(DelayedTrigger("mAttack", 0.5f));
    }

    public override void SpecialAttack2()
    {
        StartCoroutine(DelayedTrigger("Summon", 0.5f));
        GameObject Summon = Resources.Load<GameObject>("Prefab/Monster/Skeleton/TeamSkeleton3");
        Instantiate(Summon, transform.position + new Vector3(boss.dir,-2,0), Quaternion.identity);
    }

    void ResetSpeed(){
        boss.bossState.dashcoaf =1;
    }
    IEnumerator DelayedTrigger(string triggerName, float delay)
    {   
        yield return new WaitForSeconds(delay);
        anim.SetTrigger(triggerName);
    }
}
