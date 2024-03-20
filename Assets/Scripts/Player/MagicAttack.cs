using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    Rigidbody2D rigid;
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
    void Update()
    {
        //준비 시간동안 위치를 캐릭터 기준으로 설정
        if(pre == true){
            setPos();
        }
    }
    void setPos(){
        //OverlapArea로 Player을 감지하여 그 위치에 일정 값 앞에 위치시킴
        var dectectP = Physics2D.OverlapArea(rigid.position - new Vector2(3,3), rigid.position + new Vector2(3,3), LayerMask.GetMask("Player"));
        rigid.position = new Vector2(dectectP.transform.position.x + 1.7f* Dir, dectectP.transform.position.y);
    }

    void SetDir(){
        //Player가 왼쪽에 있는지 오른쪽에 있는지 감지하여 방향 값 지정
        var detectP = Physics2D.OverlapArea(rigid.position - new Vector2(3,3), rigid.position + new Vector2(3,3), LayerMask.GetMask("Player"));
        if(detectP.transform.position.x > rigid.position.x){
            Dir = -1;
        }
        else{
            Dir = 1;
        }
    }
    void SetSpeed(){
        //준비 시간이 끝난후 일정 속도로 앞으로 전진하도록함
        rigid.velocity = new Vector2(1*Dir, rigid.velocity.y);
        pre = false;
    }
    
    void Disappear(){
        Destroy(gameObject);
    }
    void OnTriggerStay2D(Collider2D col){
        //닿은오브젝트가 layer7(몬스터)이고 발사되었을 때 적용
        if(col.gameObject.layer ==7 && pre == false){
            Goblin monster = col.gameObject.GetComponent<Goblin>();
            if(monster != null){
                monster.Damaged(damage);
                Disappear();
            }
        }else if(col.gameObject.layer == 3){
            Disappear();
        }
    }
    public void setDamage(int dmg){
        damage = dmg;
    }
}
