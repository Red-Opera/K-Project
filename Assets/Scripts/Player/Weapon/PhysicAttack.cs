using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PhysicAttack : MyWeapon
{
    public override void InitSetting()
    {
        Weapon.AtkObject = Resources.Load<GameObject>("Prefab/Character/NormalAttack");
        Weapon.pos = new Vector3(1,0,0);
        Weapon.coolTime = 0.8f;
        Weapon.damage = 5;
        Weapon.disapearTime = 0.5f;    
        Weapon.folloewTime = 0.5f;
        Weapon.fowardSpeed = 1;
    }
}
