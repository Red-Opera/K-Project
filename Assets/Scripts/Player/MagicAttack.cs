using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameObject Me;
    public int Dir =1;
    public int damage =0;
    public bool pre = true;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        SetDir();
        Invoke("SetSpeed", 0.8f); //생성 모션이 완료 될때 까지 위치 고정
        Invoke("Disappear", 5); // 타켓에 맞지 않더라도 5초 뒤 사라짐
    }

    // Update is called once per frame
    void Update()
    {
        if(pre == true){
            setPos();
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.layer ==7 && pre == false){
            Scarecrow monster = col.gameObject.GetComponent<Scarecrow>();
            if(monster != null){
                monster.Damaged(damage);
                Disappear();
            }
        }else if(col.gameObject.layer == 3){
            Disappear();
        }
    }
    void SetSpeed(){
        rigid.velocity = new Vector2(1*Dir, rigid.velocity.y);
        pre = false;
    }

    void Disappear(){
        Destroy(Me);
    }

    void SetDir(){
        var detectP = Physics2D.OverlapArea(rigid.position - new Vector2(3,3), rigid.position + new Vector2(3,3), LayerMask.GetMask("Player"));
        if(detectP.transform.position.x > rigid.position.x){
            Dir = -1;
        }
        else{
            Dir = 1;
        }
    }
    public void setDamage(int dmg){
        damage = dmg;
    }

void setPos(){
        var dectectP = Physics2D.OverlapArea(rigid.position - new Vector2(3,3), rigid.position + new Vector2(3,3), LayerMask.GetMask("Player"));
        rigid.position = new Vector2(dectectP.transform.position.x + 1* Dir, dectectP.transform.position.y);
    }
}
