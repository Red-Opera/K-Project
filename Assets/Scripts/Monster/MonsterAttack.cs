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
        float BlockProbability = GameManager.info.abilityState.CEffect * (GameManager.info.abilityState.Craving/5);
        if (other.gameObject.layer == 8){
            if(Random.value < BlockProbability){
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
