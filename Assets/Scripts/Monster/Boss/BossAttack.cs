using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    Rigidbody2D rigid;
    BossSetting boss;
    SpriteRenderer spriteRenderer;
    public bool isAtk = false;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, boss.disapearTime);
        Invoke("OnAttack", 0.5f);
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

    private void OnTriggerStay2D(Collider2D other) {
        float BlockProbability = GameManager.info.abilityState.CEffect * (GameManager.info.abilityState.Craving/5);
        if(other.gameObject.layer == 8 && isAtk == true){
            if(Random.value <  BlockProbability){
                Debug.Log("Block!");
            }else{
                PMove pMove = other.gameObject.GetComponent<PMove>();
                pMove.Damaged(boss.damage);
            }
            Destroy(gameObject);
        }
    }
    void OnAttack(){
        isAtk = true;
    }
}
