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
        float AvoidProbability = GameManager.info.allPlayerState.avoidPersent;
        if (other.gameObject.layer == 8){
            if(Random.value < AvoidProbability){
                Debug.Log("Miss");
            }else{
                PMove pMove = other.gameObject.GetComponent<PMove>();
                pMove.Damaged(damage);
            }
            Destroy(gameObject);
        }
    }

    public void setDamage(int dmg){
        damage = dmg;
    }
}
