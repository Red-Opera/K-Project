using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AtkOb : MonoBehaviour
{
    Rigidbody2D rigid;
    WeaponSetting weapon;
    SpriteRenderer spriteRenderer;
    bool isFollow = true;
    int dir = 1;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDir();
        Destroy(gameObject, weapon.disapearTime);
        Invoke("endFollow", weapon.folloewTime);
    }
    void Update()
    {
        Attack();
    }
    void Attack(){
        Vector3 parentPos = transform.parent.position;
        if(isFollow == true){
            rigid.position = parentPos + weapon.pos *dir;
        }
        else{
            rigid.velocity = weapon.fowardSpeed;
            float angle = Mathf.Atan2(weapon.fowardSpeed.y, weapon.fowardSpeed.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0,angle);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 7){
            Goblin monster = other.gameObject.GetComponent<Goblin>();
            if(monster != null){
                monster.Damaged(weapon.damage);
            }
        }
    }
    void SetDir(){
        var setDir = Physics2D.OverlapArea(transform.position - new Vector3(-5,5,0), transform.position - new Vector3(0,-5,0), LayerMask.GetMask("Player"));
        if(setDir != null){
            spriteRenderer.flipX = true;
            dir = -1;
        }
    }
    void endFollow(){
        isFollow = false;
    }
    public void setState(WeaponSetting state){
        weapon = state;
    }
}
