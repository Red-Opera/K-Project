using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireCountState : MyVocation
{
    public override void InitSetting()
    {
        state.levelUpHP = 5; //5
        state.levelUpDamage = 5; //2
        state.levelUpDefense =1; //0
        state.additionalAttackSpeed = 0.75f; //1
        state.additionalCritical=0.15f; //0.05f
        state.additionalCriticalDamage = 2.5f; //1.5f
        state.weakeningCoaf = 0.3f;
        state.weakeningCoafDamage = 0.1f;
        state.drainHP = 0.05f;
    }
}
