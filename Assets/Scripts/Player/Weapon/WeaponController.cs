using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public MyWeapon myWeapon;
    public MyWeapon lastWeapon;
    public State playerState;
    SpriteRenderer spriteRenderer;
    Animator anim;
    bool isAtk = false;
    void Start()
    {
        lastWeapon = myWeapon;
        myWeapon.InitSetting();
        playerState = Resources.Load<State>("Scriptable/Player");
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(lastWeapon != myWeapon){
            lastWeapon = myWeapon;
            myWeapon.InitSetting();
        }
        if(Input.GetKeyDown(KeyCode.L)){
            Debug.Log(myWeapon.Weapon.coolTime);
        }
        if(Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.LeftControl)){
            if(isAtk == false){
                if(spriteRenderer.flipX){
                    myWeapon.Weapon.dir = -1;
                }else{
                    myWeapon.Weapon.dir = 1;
                }
                isAtk = true;
                myWeapon.Using();
                Invoke("CoolTime",myWeapon.Weapon.coolTime);
                anim.SetTrigger(myWeapon.Weapon.animName);
            }
        }
    }

    void CoolTime(){
        isAtk = false;
    }
}
