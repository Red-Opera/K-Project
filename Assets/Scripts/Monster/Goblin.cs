using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Goblin : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform trans;
    public SpriteRenderer[] childSpriterenderer;
    Animator anim;
    public MonsterState state;
    int xSpeed;
    private float moveSpeed;
    public int Hp;
    int dir;
    bool isAtk;
    float localScaleX;
    GameObject Coin;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        childSpriterenderer = GetComponentsInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        Coin = Resources.Load<GameObject>("Prefab/Object/Coin");
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        localScaleX = trans.localScale.x;
        SetState();
        SetSpeed();
        CkhGround();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
        Detect();
    }

    void Idle(){
        rigid.velocity = new Vector2(xSpeed * moveSpeed, rigid.velocity.y);
        if(xSpeed > 0){
            trans.localScale = new Vector3(-localScaleX, trans.localScale.y, 1);
            anim.SetBool("isWalk",true);
            dir = 1;
        }
        else if(xSpeed < 0){
            trans.localScale = new Vector3(localScaleX, trans.localScale.y, 1);
            anim.SetBool("isWalk",true);
            dir = -1; 
        }else{
            anim.SetBool("isWalk",false);
        }
    }
     
    void SetSpeed(){
        xSpeed = Random.Range(-1,2);
        Invoke("SetSpeed", 1.5f); 
    }

    void SetState(){
        moveSpeed = state.moveSpeed;
        Hp = state.maxHP;
    }

    public void Damaged(int dmg){
        Hp -= dmg;
        Debug.Log("Monster Damaged " + dmg + "dmg");
        StartCoroutine(setColor());
        audioSource.Play();
        if(Hp <= 0){
            StartCoroutine(SpawnCoin());
        }
    }
    IEnumerator SpawnCoin(){
        for(int i = 0; i < (state.Stage*2)+2; i++){
            Instantiate(Coin,transform.position + new Vector3(0, trans.localScale.y*.5f,0), Quaternion.identity);
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            PMove pMove = other.gameObject.GetComponent<PMove>();
            pMove.Damaged(state.damage);
        }
    }
    IEnumerator setColor(){
        foreach (SpriteRenderer renderer in childSpriterenderer)
        {
            Color originalColor = renderer.color;
            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (SpriteRenderer renderer in childSpriterenderer)
        {
            Color originalColor = renderer.color;
            renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        }
    }
    void Detect(){
        Vector2 detectRange = new Vector2(5*dir,5);
        var detectP = Physics2D.OverlapArea(rigid.position, rigid.position + detectRange, LayerMask.GetMask("Player"));
        if(detectP != null && isAtk == false){
            isAtk = true;
            Attack();
        }
    }
    void Attack(){
        GameObject range = Resources.Load<GameObject>("Prefab/Monster/MonsterAtkRange");
        range.transform.localScale = new Vector3(2.5f,2.5f,0);
        GameObject Atk = Instantiate(range, transform.position + new Vector3(1.5f,-0.5f,0)* dir, Quaternion.identity);
        MonsterAttack atkSc = Atk.GetComponent<MonsterAttack>();
        atkSc.setDamage(state.damage);
        anim.SetTrigger("isAtk");
        Invoke("AtkReset",3);
    }

    void AtkReset(){
        isAtk = false;
    }
    void CkhGround(){
        // 이동 방향에 따른 Raycast 위치 설정
        Vector2 rayOriginFront = new Vector2(rigid.position.x + dir * 0.5f, rigid.position.y);
        Vector2 rayOriginBottom = new Vector2(rigid.position.x, rigid.position.y);

        // 전방에 platform이 있는지 확인
        RaycastHit2D platformHitFront = Physics2D.Raycast(
            rayOriginFront,
            Vector2.down,
            1,
            LayerMask.GetMask("Platform")
        );

        // Goblin이 땅에 있는지 확인
        RaycastHit2D groundHit = Physics2D.Raycast(
            rayOriginBottom,
            Vector2.down,
            1,
            LayerMask.GetMask("Platform")
        );

        // 전방에 platform이 없거나 Goblin이 땅에 있지 않을 경우 방향 전환
        if (platformHitFront.collider == null || groundHit.collider == null)
        {
            xSpeed *= -1;
            dir *= -1;
            trans.localScale = new Vector3(localScaleX * dir, trans.localScale.y, 1);
            Debug.Log("change");
        }
        Debug.DrawLine(rayOriginFront, rayOriginFront + Vector2.down, Color.red);
        Debug.DrawLine(rayOriginBottom, rayOriginBottom + Vector2.down, Color.blue);
        Invoke("CkhGround", 0.1f);
    }
}
