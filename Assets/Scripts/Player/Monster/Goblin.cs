using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public int xSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        SetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
    }

    void Idle(){
        rigid.velocity = new Vector2(xSpeed, rigid.velocity.y);
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
            Debug.Log("is Stop");
        }
        else{
            anim.SetBool("isWalk",true);
            Debug.Log("is Walking");
        }
        
    }
}
