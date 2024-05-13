using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public HpLevelManager hpLevelManager;
    int damage;
    // Start is called before the first frame update
    void Start()
    {
        hpLevelManager = FindObjectOfType<HpLevelManager>();
        Destroy(gameObject, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.layer == 8){
            GameManager.info.allPlayerState.currentHp -= damage;
            hpLevelManager.Damage();
            Destroy(gameObject);
        }
    }

    public void setDamage(int dmg){
        damage = dmg;
    }
}
