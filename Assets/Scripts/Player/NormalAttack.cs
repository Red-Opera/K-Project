using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NormalAttack : MonoBehaviour
{
    public GameObject Me;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    int Dir = 1;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        SelecteDirection();
        Invoke("Delete", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        setPos();
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

    void SelecteDirection(){
        var setDir = Physics2D.OverlapArea(transform.position - new Vector3(-5,5,0), transform.position - new Vector3(0,-5,0), LayerMask.GetMask("Player"));
        if(setDir != null){
            spriteRenderer.flipX =true;
            Dir = -1;
        }
    }

    void setPos(){
        Vector2 parentPos = rigid.transform.parent.position;
        rigid.position = parentPos + new Vector2(0.5f * Dir,0);
    }
}
