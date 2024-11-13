using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PMove : MonoBehaviour
{
    Rigidbody2D rigid;
    CapsuleCollider2D PlayerCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public HpLevelManager hpLevelManager;
    public ResultUI result;
    public State playerState;
    private bool isJumping;
    private bool isAttack = false;
    private bool usingUI = false;
    private GameObject interactiveUi;
    private Dialog dialog;
    private Coroutine uiCheckCoroutine;
    private GameObject hpBar;
    public bool isRevive = true;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        Invoke("FindUI",0.1f);
        DontDestroyOnLoad(gameObject);
    }
    void Start(){
        playerState = GameManager.info.allPlayerState;
        ResetStat();
        SceneManager.sceneLoaded += reload;
        Invoke("RefillShield", 10);
    }

    public void FindUI(){
        Debug.Log("Find UI");
        hpBar = GameObject.FindGameObjectWithTag("HP");
        if (hpBar != null){
            Debug.Log("hpBar is not null");
            hpLevelManager = hpBar.GetComponent<HpLevelManager>();
        }else{
            Debug.Log("hpBar is null");
        }
        result = FindObjectOfType<ResultUI>();
    }
    void Update()
    {
        if(usingUI == false){
            Move();
            Jump();
            Dash();
        }
        if(playerState.currentHp <= 0 && gameObject.layer != 12){
            if(isRevive && (GameManager.info.abilityState.Craving/5) > 0){
                float reviveHpCoaf = GameManager.info.abilityState.CEffect * ( GameManager.info.abilityState.Craving / 5);
                GameManager.info.allPlayerState.currentHp = (int)(GameManager.info.allPlayerState.maxHP * reviveHpCoaf);
                isRevive = false;
                hpLevelManager.Damage();
            }
            else{
                Die();
                isRevive = true;
            }
        }
    }

    void Move(){
        float Horiz = Input.GetAxisRaw("Horizontal");
        if(Horiz != 0){
            if(rigid.velocity.x>0){
                spriteRenderer.flipX = false;
            }else if(rigid.velocity.x<0){
                spriteRenderer.flipX = true;
            }
            anim.SetBool("isWalk",true);
            if(Input.GetKey(KeyCode.LeftShift)){
                rigid.velocity = new Vector2(Horiz * playerState.moveSpeed * 1.5f, rigid.velocity.y);
                if(rigid.velocity.x != 0){
                    anim.SetBool("isRun",true);
                }else{
                    anim.SetBool("isRun",false);
                }if(rigid.velocity.x>0){
                    spriteRenderer.flipX = false;
                }else if(rigid.velocity.x<0){
                    spriteRenderer.flipX = true;
                }
            }else{
                anim.SetBool("isRun",false);
                rigid.velocity = new Vector2(Horiz * playerState.moveSpeed, rigid.velocity.y);
            
            }
        }
        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun",false);
        }
        
    }
    void Jump(){
        float ColliderSizeX = PlayerCollider.size.x/2;
        float ColliderSizeY = PlayerCollider.size.y/2 -PlayerCollider.offset.y;

        if(Input.GetButtonDown("Jump") && playerState.jumpCount < playerState.maxJump){
            rigid.velocity = new Vector2(rigid.velocity.x, playerState.jumpPower);
            playerState.jumpCount ++;
            isJumping = true;
            Invoke("OnJump", 0.03f);
        }
        Vector2 PlayerPos = new Vector2(transform.position.x, transform.position.y);
        var GroundHit = Physics2D.OverlapArea(PlayerPos - new Vector2(ColliderSizeX,ColliderSizeY), PlayerPos - new Vector2(-ColliderSizeX,ColliderSizeY),LayerMask.GetMask("Platform","DamagedObject"));
        if(GroundHit != null && isJumping == false){
            anim.SetBool("isJump",false);
            if(playerState.jumpCount > 0){
                playerState.jumpCount = 0;
            }
        }

        if(rigid.velocity.y > playerState.jumpPower*3){
            rigid.velocity = new Vector2(rigid.velocity.x, playerState.jumpPower*3);
        }
        
    }
    void OnJump(){
        isJumping = false;
        anim.SetBool("isJump",true);
    }

    void Dash(){
        DashUI dashUI = FindObjectOfType<DashUI>();
        if(Input.GetMouseButtonDown(1) && dashUI.dashBarSlider.value >= 0.2){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dashDir = mousePos - rigid.position;
            rigid.velocity += dashDir.normalized* 8;
            if(rigid.velocity.y >= playerState.jumpPower*5){
                rigid.velocity = new Vector2(rigid.velocity.x, playerState.jumpPower*5);
            }
            dashUI.DashUIApply();

            if(dashDir.x >0){
                spriteRenderer.flipX = false;
            }
            else if(dashDir.x <0){
                spriteRenderer.flipX = true;
            }
 
            gameObject.layer = 9;
            Invoke("OnJump", 0.03f);
            Invoke("DashEnd",0.8f);
        }
    }
        void DashEnd(){
        gameObject.layer = 8;
    }
    void Die(){
        anim.SetTrigger("Die");
        gameObject.layer = 12;
        ResultUI result = FindObjectOfType<ResultUI>();
        result.GameIsEnd();
        WeaponController weaponController= FindObjectOfType<WeaponController>();
        weaponController.enabled = false;
        enabled = false;

        SceneManager.sceneLoaded -= reload;
    }
    public void Damaged(int dmg){
        gameObject.layer = 12;
        int Damage = dmg - GameManager.info.allPlayerState.defense;
        if(Damage <= 0 ){
            Damage =1;
        }
        GameManager.info.allPlayerState.currentHp -= Damage;
        hpLevelManager.Damage();
        Invoke("DashEnd", 0.5f);
    }
    void reload(Scene scene, LoadSceneMode mode)
    {
        FindUI();
    }

    public void SendUI(GameObject openUI){
        interactiveUi = openUI;
        if (interactiveUi.tag == "Immovable"){
            usingUI = true;
            StopPlayerMovement();

            uiCheckCoroutine = StartCoroutine(ChkUiState());
        }
    }
    public void SendDialog(Dialog chat){
        dialog = chat;
        usingUI = true;
        StopPlayerMovement();

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

    void StopPlayerMovement(){
        anim.SetBool("isWalk", false);
        anim.SetBool("isRun",false);
        anim.SetBool("isJump",false);
        rigid.velocity = new Vector2(rigid.velocity.x * 0.1f, rigid.velocity.y);
    }

    void ResetStat(){
        GameManager.info.abilityState.Anger = 0;
        GameManager.info.abilityState.Haste = 0;
        GameManager.info.abilityState.Patience = 0;
        GameManager.info.abilityState.Mystery = 0;
        GameManager.info.abilityState.Greed = 0;
        GameManager.info.abilityState.Craving = 0;
        GameManager.info.abilityState.shield = 0;
    }

    void RefillShield(){
        int maxShield = (int)(GameManager.info.abilityState.PEffect * (GameManager.info.abilityState.Patience/5)*GameManager.info.allPlayerState.maxHP);
        if(GameManager.info.abilityState.shield < maxShield){
            if((maxShield - GameManager.info.abilityState.shield)<=(maxShield * 0.1)){
                GameManager.info.abilityState.shield = maxShield;
            }else{
                GameManager.info.abilityState.shield += (int)(maxShield * 0.1);
            }
        }
    }
}
