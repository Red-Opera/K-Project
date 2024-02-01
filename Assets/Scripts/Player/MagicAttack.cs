using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameObject Me;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("SetSpeed", 0.8f);
        Invoke("Disappear", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.layer ==7){
            Scarecrow monster = col.gameObject.GetComponent<Scarecrow>();
            if(monster != null){
                monster.Damaged(18);
                Disappear();
            }
        }
    }
    void SetSpeed(){
        rigid.velocity = new Vector2(1, rigid.velocity.y);
    }

    void Disappear(){
        Destroy(Me);
    }
}
