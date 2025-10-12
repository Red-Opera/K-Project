using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire_TransitionAttack : MyWeapon
{
    public override void InitSetting()
    {
        Weapon.AtkObject = Resources.Load<GameObject>("Prefab/Character/VampireAttack");
        Weapon.AtkEffect = Resources.Load<GameObject>("Prefab/SkillEffect/impactbluemedium");
        Weapon.pos = new Vector3(1,0,0);
        Weapon.coolTime = 0.8f;
        Weapon.damage = GameManager.info.allPlayerState.damage;
        Weapon.disapearTime = 0.5f;    
        Weapon.followTime = 0.5f;
        Weapon.forwardSpeed = new Vector2(0,0);
        Weapon.animName = "TAttack";
    }
}
