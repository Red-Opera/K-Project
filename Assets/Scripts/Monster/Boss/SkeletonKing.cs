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
        boss.damage = 10;
        boss.disapearTime = .3f;
        anim = GetComponent<Animator>();
    }
    public override void Attack1()
    {
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir,Quaternion.identity );
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        anim.SetTrigger("Attack");
    }

    public override void Attack2()
    {
        boss.ainmterm = 0;
        boss.bossState.dashcoaf =2;
        Invoke("ResetSpeed", 0.5f);
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        anim.SetTrigger("Dash");
    }

    public override void SpecialAttack1()
    {
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        anim.SetTrigger("mAttack");
    }

    public override void SpecialAttack2()
    {
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir,Quaternion.identity );
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss);
        anim.SetTrigger("Summon");
    }

    void ResetSpeed(){
        boss.bossState.dashcoaf =1;
    }
}
