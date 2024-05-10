using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpear : MyWeapon
{
    public override void InitSetting()
    {
        Weapon.AtkObject = Resources.Load<GameObject>("Prefab/Character/MagicSpear");
        Weapon.pos = new Vector3(1.7f,0,0);
        Weapon.coolTime = 0.75f;
        Weapon.damage = 5;
        Weapon.disapearTime = 5;    
        Weapon.folloewTime = 0.8f;
        Weapon.fowardSpeed = new Vector2(1,0);
        Weapon.animName = "mAttack";
    }
}
