using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public struct WeaponSetting{
    public GameObject AtkObject;
    public Vector3 pos;
    public float coolTime;
    public int damage;
    public float disapearTime;
    public float folloewTime;
    public Vector2 fowardSpeed;
    public string animName;
    public int dir;
}

public abstract class MyWeapon : MonoBehaviour
{
    public WeaponSetting Weapon;
    public abstract void InitSetting();
    public virtual void Using(){
        GameObject Atk = Instantiate(Weapon.AtkObject, transform.position + Weapon.pos * Weapon.dir, Quaternion.identity);
        Atk.transform.SetParent(transform);
        AtkOb AtkSc = Atk.GetComponent<AtkOb>();
        AtkSc.setState(Weapon);
    }
}
