using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class AtkOb : MonoBehaviour
{
    Rigidbody2D rigid;
    WeaponSetting weapon;
    bool isFollow = true;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); 
        Destroy(gameObject, weapon.disapearTime);
        Invoke("endFollow", weapon.folloewTime);
    }
    void Update()
    {
        Attack();
        Debug.Log("11");
    }
    void Attack(){
        Vector3 parentPos = transform.parent.position;
        if(isFollow == true){
            rigid.position = parentPos + weapon.pos;
        }
        else{
            rigid.velocity = new Vector2(weapon.fowardSpeed,0);
        }
    }
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.gameObject.layer == 7){
    //         Goblin monster = other.gameObject.GetComponent<Goblin>();
    //         if(monster != null){
    //             monster.Damaged(weapon.damage);
    //         }
    //     }
    // }
    void endFollow(){
        isFollow = false;
    }
    public void setState(WeaponSetting state){
        weapon = state;
    }
}
