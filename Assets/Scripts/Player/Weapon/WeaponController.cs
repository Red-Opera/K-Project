using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public MyWeapon myWeapon;
    public State playerState;
    public MyWeapon[] skillList;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audioSource;
    public Rigidbody2D rigid;
    bool isAtk = false;
    int skillCount = 0;
    void Start()
    {
        myWeapon.InitSetting();
        playerState = Resources.Load<State>("Scriptable/Player");
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.LeftControl)){
            if(isAtk == false){
                myWeapon.InitSetting();
                if(spriteRenderer.flipX){
                    myWeapon.Weapon.dir = -1;
                }else{
                    myWeapon.Weapon.dir = 1;
                }
                CheckMousePos();
                isAtk = true;
                myWeapon.Using();
                audioSource.Play();
                Invoke("CoolTime",myWeapon.Weapon.coolTime);
                anim.SetTrigger(myWeapon.Weapon.animName);
            }
        }
        CheckSkillChange();
    }

    void CoolTime(){
        isAtk = false;
    }
    void CheckMousePos(){
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isMouse = false;
        if(myWeapon.Weapon.dir ==1){
            isMouse = (mousePos.x > rigid.position.x);
        } else{
            isMouse = (mousePos.x < rigid.position.x);
        }
        if(isMouse){
            Vector2 attackSpeed = mousePos - rigid.position;
            myWeapon.Weapon.fowardSpeed = attackSpeed.normalized *2;
        }
        else{
            myWeapon.Weapon.fowardSpeed = new Vector2(myWeapon.Weapon.dir * 2,0);
        }
    }

    void CheckSkillChange(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            skillCount++;
            myWeapon = skillList[skillCount%skillList.Length];
            myWeapon.InitSetting();
        }
    }
}
