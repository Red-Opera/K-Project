using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MyWeapon
{
    public override void InitSetting()
    {
        Weapon.AtkObject = Resources.Load<GameObject>("Prefab/Character/MagicBall");
        Weapon.AtkEffect = Resources.Load<GameObject>("Prefab/SkillEffect/impactbluesmall");
        Weapon.pos = new Vector3(1.7f,0,0);
        Weapon.coolTime = 0.75f;
        Weapon.damage = GameManager.info.allPlayerState.damage;
        Weapon.disapearTime = 5;    
        Weapon.folloewTime = 0.8f;
        Weapon.fowardSpeed = new Vector2(1.5f,0);
        Weapon.animName = "mBall";
    }
}
