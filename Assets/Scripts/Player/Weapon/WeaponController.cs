using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public MyWeapon myWeapon;
    public MyWeapon lastWeapon;
    public State playerState;
    bool isAtk = false;
    void Start()
    {
        lastWeapon = myWeapon;
        myWeapon.InitSetting();
        playerState = Resources.Load<State>("Scriptable/Player");
    }
    void Update()
    {
        if(lastWeapon != myWeapon){
            lastWeapon = myWeapon;
            myWeapon.InitSetting();
        }
        if(Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.LeftControl)){
            if(isAtk == false){
                isAtk = true;
                myWeapon.Using();
                Invoke("CoolTime",myWeapon.Weapon.coolTime);
            }
        }
    }

    void CoolTime(){
        isAtk = false;
    }
}
