using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public struct WeaponSetting{
    public GameObject AtkObject;
    public Vector3 pos;
    public float coolTime;
    public int damage;
    public float disapearTime;
    public float folloewTime;
    public float fowardSpeed;
}

public abstract class MyWeapon : MonoBehaviour
{
    public WeaponSetting Weapon;
    public abstract void InitSetting();
    public virtual void Using(){
        GameObject Atk = Instantiate(Weapon.AtkObject);
        Atk.transform.SetParent(transform);
        AtkOb AtkSc = Atk.GetComponent<AtkOb>();
        AtkSc.setState(Weapon);
    }
}
