using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    int ySpeed;
    int xSpeed;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        setSpeed();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();   
    }

    void setSpeed(){
        ySpeed = Random.Range(-1,2);
        xSpeed = Random.Range(-1,2);
        Debug.Log(ySpeed);
        Invoke("setSpeed", 1.5f);
    }

    void Idle(){
        rigid.velocity = new Vector2(xSpeed,ySpeed * 0.8f);
        if(xSpeed > 0){
            spriteRenderer.flipX = false;
        }
        else if(xSpeed < 0){
            spriteRenderer.flipX = true;
        }
    }
}
