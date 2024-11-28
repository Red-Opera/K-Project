using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirePeacookState : MyVocation
{
    public override void InitSetting()
    {
        state.levelUpHP = 5; //5
        state.levelUpDamage = 4; //2
        state.levelUpDefense =1; //0
        state.additionalAttackSpeed = 1.5f; //1
        state.additionalCritical=0.25f; //0.05f
        state.additionalCriticalDamage = 2f; //1.5f
        state.drainHP = 0.05f;
        state.bleedingCoaf = 0.01f;
    }
}
