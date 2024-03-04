using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public State state;
    public int xSpeed;
    private float moveSpeed;
    public int Hp;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
            spriteRenderer.flipX = false;
        }
        else if(xSpeed < 0){
            spriteRenderer.flipX = true;
        }
    }
     
    void SetSpeed(){
        xSpeed = Random.Range(-1,2);
        Invoke("SetSpeed", 1.5f);
        if(xSpeed == 0 ){
            anim.SetBool("isWalk",false);
        }
        else{
            anim.SetBool("isWalk",true);
        }   
    }

    void SetState(){
        moveSpeed = state.moveSpeed;
        Hp = state.maxHP;
        
    }

    public void Damaged(int dmg){
        Hp -= dmg;
        Debug.Log("Monster Danaged " + dmg + "dmg");
    }
}
