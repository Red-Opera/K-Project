using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WareWolves : BossMonster
{
    public override void InitSetting()
    {
        boss.range = Resources.Load<GameObject>("Prefab/Boss/AttackRange");
        boss.pos = new Vector3(2, 0, 0);
        boss.damage = 10;
        boss.disapearTime = 0.3f;
    }
    public override void Attack1()
    {
        Debug.Log("Attack1");
        GameObject Atk = Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
        BossAttack atkSc = Atk.GetComponent<BossAttack>();
        atkSc.setState(boss); 
    }

    public override void Attack2()
    {
        Debug.Log("Attack2");
        Instantiate(boss.range, transform.position + boss.pos*boss.dir, Quaternion.identity);
    }
}
