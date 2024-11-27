using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantressState : MyVocation
{
    public override void InitSetting()
    {
        state.levelUpHP = 6; //5
        state.levelUpDamage = 5; //2
        state.levelUpDefense =0; //0
        state.additionalAttackSpeed = 0.5f; //1
        state.additionalCritical=0.15f; //0.05f
        state.additionalCriticalDamage = 3f; //1.5f
        state.weakeningCoaf = 0;
        state.weakeningCoafDamage = 0;
    }
}
