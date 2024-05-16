using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ckhMap : MonoBehaviour
{
    Rigidbody2D rigid;
    public BossController riffer;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        riffer = GetComponent<BossController>();
    }

    // Update is called once per frame
    void Update()
    {
        MapIn();
    }

    void MapIn(){
        if(rigid.position.x >= 415 && rigid.velocity.x > 0){
            riffer.moveSpeed *= -1;
        }else if(rigid.position.x <= 385 && rigid.velocity.x < 0){
            riffer.moveSpeed *= -1;
        }
        if(rigid.position.y >= 28 && rigid.velocity.y > 0){
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y *-1);
        }else if(rigid.position.y <= 18 && rigid.velocity.y < 0){
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y *-1);
        }
    }
}
