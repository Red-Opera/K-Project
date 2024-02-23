using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public int xSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();   
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
    }
}
