using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool playerCheck;
    PlatformEffector2D platformObject;

    void Start ()
    {
        playerCheck = false;
        platformObject = GetComponent<PlatformEffector2D>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) && playerCheck)
        {
            platformObject.rotationalOffset= 180f;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            platformObject.rotationalOffset = 0;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        playerCheck = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        playerCheck = false;
    }

}
    
    