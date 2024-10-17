using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAnimRun();
    }

    void CheckAnimRun(){
        AnimatorStateInfo stateInfo= anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1.0f && !anim.IsInTransition(0)){
            Destroy(gameObject);
        }
    }
}
