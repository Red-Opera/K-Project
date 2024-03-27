using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NormalAttack : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    int Dir = 1;
    public int Damage = 0;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        SelecteDirection();
        Invoke("Delete", 0.5f);
    }

    void Update()
    {
        setPos();
    }

    void Delete(){
        Destroy(gameObject);
    }

    void SelecteDirection(){
        // 플레이어 위치를 감지하여 공격 방향 설정
        var setDir = Physics2D.OverlapArea(transform.position - new Vector3(-5,5,0), transform.position - new Vector3(0,-5,0), LayerMask.GetMask("Player"));
        if(setDir != null){
            spriteRenderer.flipX =true;
            Dir = -1;
        }
    }

    void setPos(){
        //부모 객체의 위치 값을 받아와서 공격이 플레이어를 계속 따라가도록 함
        Vector2 parentPos = rigid.transform.parent.position;
        rigid.position = parentPos + new Vector2(0.8f * Dir,0);
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.layer == 7){
            Goblin monster = col.gameObject.GetComponent<Goblin>();
            if(monster != null){
                monster.Damaged(Damage);
            }
        }
    }
    public void setDamage(int Dmg){
        Damage = Dmg;
    }
}
