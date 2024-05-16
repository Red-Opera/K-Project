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
    public bool isJumping;
    public bool isAttack = false;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        PlayerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        result = FindObjectOfType<ResultUI>();
        GameObject hpBar = GameObject.FindGameObjectWithTag("HP");
        hpLevelManager = hpBar.GetComponent<HpLevelManager>();
        DontDestroyOnLoad(gameObject);
    }
    void Start(){
        playerState = GameManager.info.allPlayerState;
        SceneManager.sceneLoaded += reload;
    }


    void Update()
    {
        Move();
        Jump();
        Dash();
        if(playerState.currentHp <= 0 && gameObject.layer != 12){
            Die();
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
    }
    public void Damaged(int dmg){
        Debug.Log(dmg);
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
        Awake();
    }
}
