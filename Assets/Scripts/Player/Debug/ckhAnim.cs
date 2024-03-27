using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ckhAnim : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q)){
            anim.SetBool("isWalk",true);
        }
        else{
            anim.SetBool("isWalk",false);
        }
        if(Input.GetKey(KeyCode.W)){
            anim.SetBool("isAtk",true);
        }
        else{
            anim.SetBool("isAtk",false);
        }
    }
}
