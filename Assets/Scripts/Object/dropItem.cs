using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dropItem : MonoBehaviour
{
    Rigidbody2D rigid;
    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 3,ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 8){
            Destroy(gameObject);
        }
    }
}
