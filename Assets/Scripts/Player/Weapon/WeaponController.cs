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
    float dobleShotProbability;

    GameObject interactiveUi;
    private bool usingUI = false;
    private Coroutine uiCheckCoroutine;
    private Dialog dialog;
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
        if(usingUI == false){
            Attack();
            CheckSkillChange();
        }
    }
    void Attack(){
        if(Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.LeftControl)){
            if(isAtk == false){
                dobleShotProbability = (GameManager.info.abilityState.Haste/5) * GameManager.info.abilityState.HEffect;
                myWeapon.InitSetting();
                if(spriteRenderer.flipX){
                    myWeapon.Weapon.dir = -1;
                }else{
                    myWeapon.Weapon.dir = 1;
                }
                CheckMousePos();
                isAtk = true;
                AttackStart();
                if(Random.value < dobleShotProbability){
                    Invoke("AttackStart", 0.5f);
                }
                Invoke("CoolTime",myWeapon.Weapon.coolTime / GameManager.info.allPlayerState.attackSpeed);
                anim.SetTrigger(myWeapon.Weapon.animName);
            }
        }
    }

    void AttackStart(){
        myWeapon.Using();
        audioSource.Play();
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

    public void SendUI(GameObject openUI){
        interactiveUi = openUI;
        if (interactiveUi.tag == "Immovable"){
            usingUI = true;
            
            uiCheckCoroutine = StartCoroutine(ChkUiState());
        }
    }

    public void SendDialog(Dialog chat){
        dialog = chat;
        usingUI = true;
        uiCheckCoroutine = StartCoroutine(ChkDialogState());
    }

    IEnumerator ChkUiState(){
        while(interactiveUi.activeSelf){
            yield return null;
        }

        usingUI = false;
    }

    IEnumerator ChkDialogState(){
        while(dialog.isActiveAndEnabled){
            yield return null;
        }

        usingUI = false;
    }
}
