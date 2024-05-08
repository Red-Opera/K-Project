using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    Rigidbody2D rigid;
    BossSetting boss;
    SpriteRenderer spriteRenderer;
    public HpLevelManager hpLevelManager;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hpLevelManager = FindObjectOfType<HpLevelManager>();
        Destroy(gameObject, boss.disapearTime);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        1. 보스 공격 간격 조정
        2. 보스 공격 전 공격 범위 생성
        5. 원거리 공격 마우스 위치로 발사
        6. 죽으면 결과 Ui 생성
        7. 피격 효과음
        */
    }
    void setDir(){
        var setD = Physics2D.OverlapArea(rigid.position - new Vector2(-5,5),rigid.position + new Vector2(0,-5), LayerMask.GetMask("Boss"));
        if(setD != null){
            spriteRenderer.flipX =true;
        }
    }
    public void setState(BossSetting setting){
        boss = setting;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 9){
            GameManager.info.playerState.currentHp -= boss.damage;
            hpLevelManager.Damage();
        }
    }
}
