using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Goblin : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform trans;
    public GameManager gameManager;
    public HpLevelManager hpLevelManager;
    public SpriteRenderer[] childSpriterenderer;
    //Animator anim;
    public MonsterState state;
    public int xSpeed;
    private float moveSpeed;
    public int Hp;
    public int dir;
    GameObject Coin;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        hpLevelManager = FindObjectOfType<HpLevelManager>();
        childSpriterenderer = GetComponentsInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        Coin = Resources.Load<GameObject>("Prefab/Object/Coin");
        // anim = GetComponent<Animator>();
        SetState();
        SetSpeed();
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
            trans.localScale = new Vector3(-1.5f, 1.7f, 1); 
            dir = 1;
        }
        else if(xSpeed < 0){
            trans.localScale = new Vector3(1.5f, 1.7f, 1);
            dir = -1; 
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
        if(Hp <= 0){
            Instantiate(Coin,transform.position + new Vector3(0, trans.localScale.y*.5f,0), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            gameManager.playerState.currentHp -= state.damage;
            hpLevelManager.Damage();
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
        Vector2 detectRange = new Vector2(10*dir,5);
        var detectP = Physics2D.OverlapArea(rigid.position, rigid.position + detectRange, LayerMask.GetMask("Player"));
        if(detectP != null){
            Debug.Log("Find Player");
        }
    }
}
