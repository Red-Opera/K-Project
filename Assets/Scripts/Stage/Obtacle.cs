using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obtacle : MonoBehaviour
{
    HpLevelManager hpLevelManager;
    State state;

    public int damage = 20;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PMove pMove = other.gameObject.GetComponent<PMove>();
            pMove.Damaged(damage);
        }
    }
}

