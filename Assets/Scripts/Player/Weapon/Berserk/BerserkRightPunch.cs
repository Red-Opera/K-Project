using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkRightPunch : MyWeapon
{
    public override void InitSetting()
    {
        Weapon.AtkObject = Resources.Load<GameObject>("Prefab/Character/NormalAttack");
        Weapon.AtkEffect = Resources.Load<GameObject>("Prefab/SkillEffect/impactredsmall");
        Weapon.pos = new Vector3(1, 0, 0);
        Weapon.ObjectSize = new Vector3(3f, 2f, 1f);
        Weapon.coolTime = 0.8f;
        Weapon.damage = GameManager.info.allPlayerState.damage;
        Weapon.disapearTime = 0.5f;    
        Weapon.followTime = 0.5f;
        Weapon.forwardSpeed = new Vector2(0,0);
        Weapon.animName = "RPunch";
        Weapon.dashPower = 2f;
    }
}
