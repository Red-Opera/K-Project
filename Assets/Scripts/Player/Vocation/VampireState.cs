using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireState : MyVocation
{
    public override void InitSetting()
    {
        state.levelUpHP = 5; //5
        state.levelUpDamage = 3; //2
        state.levelUpDefense =1; //0
        state.additionalAttackSpeed = 1; //1
        state.additionalCritical=0.05f; //0.05f
        state.additionalCriticalDamage = 1.5f; //1.5f
        state.drainHP = 0.05f;
    }
}
