using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform trans;
    public GameManager gameManager;
    public HpLevelManager hpLevelManager;
    //Animator anim;
    public MonsterState state;
    public int xSpeed;
    private float moveSpeed;
    public int Hp;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        hpLevelManager = FindObjectOfType<HpLevelManager>();
        // hpLevelManager = GetComponentInChildren<HpLevelManager>();
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        // anim = GetComponent<Animator>();
        SetState();
        SetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
    }

    void Idle(){
        rigid.velocity = new Vector2(xSpeed * moveSpeed, rigid.velocity.y);
        if(xSpeed > 0){
            trans.localScale = new Vector3(-1.5f, 1.7f, 1); 
        }
        else if(xSpeed < 0){
            trans.localScale = new Vector3(1.5f, 1.7f, 1); 
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
        setColor();
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            gameManager.playerState.currentHp -= state.damage;
            hpLevelManager.Damage();
        }
    }
    void setColor(){
        Debug.Log("isOn");
        foreach (Transform child in transform)
        {
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                childRenderer.color = Color.red;
            }
        }
    }
}
