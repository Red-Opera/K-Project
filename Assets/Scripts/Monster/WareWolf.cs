using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class WareWolf : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public MonsterState state;
    int moveSpeed;
    int dir =1;
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = Resources.Load<MonsterState>("Scriptable/Boss/WareWolf");
        SetSpeed();
    }
    void Update()
    {
        Idle();
        DetectP();
    }
    void SetSpeed(){
        moveSpeed = Random.Range(-1, 2);
        Invoke("SetSpeed", 1.5f);
    }

    void Idle(){
        rigid.velocity = new Vector2 (moveSpeed * state.moveSpeed,rigid.velocity.y);
        if(rigid.velocity.x > 0){
            spriteRenderer.flipX = false;
            anim.SetBool("isWalk", true);
            dir =1;
        }else if(rigid.velocity.x <0){
            spriteRenderer.flipX = true;
            anim.SetBool("isWalk", true);
            dir = -1;
        }else{
            anim.SetBool("isWalk",false);
        }
    }

    void DetectP(){
        Vector2 detectRange = new Vector2(10*dir, 5);
        var detectP = Physics2D.OverlapArea(detectRange, rigid.position, LayerMask.GetMask("Player"));
    }
}
