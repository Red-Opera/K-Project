using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{

    public int health = 10000;
    void Update()
    {
        
    }

    public void Damaged(int dmg){
        health -=dmg;
        Debug.Log(dmg);
    }
}
