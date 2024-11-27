using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkState : MyVocation
{
    public override void InitSetting()
    {
        state.levelUpHP = 6; //5
        state.levelUpDamage = 4; //2
        state.levelUpDefense =1; //0
        state.additionalAttackSpeed = 1.2f; //1
        state.additionalCritical=0.25f; //0.05f
        state.additionalCriticalDamage = 2; //1.5f
        state.weakeningCoaf = 0;
        state.weakeningCoafDamage = 0;
    }
}
