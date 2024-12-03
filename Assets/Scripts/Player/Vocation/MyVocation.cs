using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct VacationSetting{
    public int levelUpHP;
    public int levelUpDamage;
    public int levelUpDefense;
    public float additionalAttackSpeed;
    public float additionalCritical;
    public float additionalCriticalDamage;
    public float weakeningCoaf;
    public float weakeningCoafDamage;
    public float drainHP;
    public float bleedingCoaf;
}
public abstract class MyVocation : MonoBehaviour
{
    public VacationSetting state;
    public abstract void InitSetting();
    private void SetVacationState()
    {
        GameManager.info.allPlayerState.currentHp = GameManager.info.allPlayerState.maxHP;
        GameManager.info.addLevelState.maxHP = state.levelUpHP;
        GameManager.info.addLevelState.damage = state.levelUpDamage;
        GameManager.info.addLevelState.defense = state.levelUpDefense;
        GameManager.info.playerState.attackSpeed = state.additionalAttackSpeed;
        GameManager.info.playerState.critical = state.additionalCritical;
        GameManager.info.playerState.criticalDamage = state.additionalCriticalDamage;
    }

    public void UpdateVocationState(){
        SetVacationState();
        GameManager.info.UpdatePlayerState();
    }
}
