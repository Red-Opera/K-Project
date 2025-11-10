using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHitDown : MyWeapon
{
    public override void InitSetting()
    {
        Weapon.AtkObject = Resources.Load<GameObject>("Prefab/Character/NormalAttack");
        Weapon.AtkEffect = Resources.Load<GameObject>("Prefab/SkillEffect/bloodbig");
        Weapon.pos = new Vector3(1,0,0);
        Weapon.coolTime = 0.8f;
        Weapon.damage = GameManager.info.allPlayerState.damage;
        Weapon.disapearTime = 0.5f;    
        Weapon.followTime = 0.5f;
        Weapon.forwardSpeed = new Vector2(0,0);
        Weapon.animName = "HitDown";
        Weapon.dashPower = 4f;
    }
}
