using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dropItem : MonoBehaviour
{
    Rigidbody2D rigid;
    int Coin;
    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(3, Random.Range(-1,2)),ForceMode2D.Impulse);
        Coin = Random.Range(8, 12);
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 8){
            Destroy(gameObject);
            ResultUI.GetGold(Coin);
        }
    }
}
