using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.layer == 7){
            Scarecrow scarecrow = col.gameObject.GetComponent<Scarecrow>();
            if(scarecrow != null){
                scarecrow.Damaged(18);
            }
        }
    }
}
