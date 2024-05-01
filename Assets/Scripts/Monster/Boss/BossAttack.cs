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
