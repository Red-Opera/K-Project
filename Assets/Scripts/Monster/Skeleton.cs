using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    Rigidbody2D rigid;
    RectTransform rectTransform;
    public int xSpeed;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();
        SetSpeed();
    }
    void Update()
    {
        Idle();
        Detect();
    }

    void SetSpeed(){
        //Random 함수를 사용해 이동속도를 지정
        xSpeed = Random.Range(-1,2);
        Invoke("SetSpeed",1.5f);
    }
    void Idle(){
        //Random 함수로 받아온 값으로 이동하도록 함
        rigid.velocity = new Vector2(xSpeed,rigid.velocity.y);
        //xSpeed을 바탕으로 rotation 값을 조정하여 바라보는 방향을 조정
        if(xSpeed >0){
            rectTransform.rotation = Quaternion.Euler(0,180,0);
        }
        else if(xSpeed <0){
            rectTransform.rotation = Quaternion.Euler(0,0,0);
        }
    }

    void Detect(){
        Vector2 detectRange = new Vector2(10,5);
        //지정한 범위내에 플레이어가 있을시에 감지하여 로그 출력
        var detectP = Physics2D.OverlapArea(rigid.position - detectRange, rigid.position + detectRange, LayerMask.GetMask("Player"));
        if(detectP != null){
            Debug.Log("Find Player");
        }
    }
}
