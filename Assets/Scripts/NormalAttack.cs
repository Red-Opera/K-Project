using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    public GameObject Me;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Delete", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Delete(){
        Destroy(Me);
    }
}
