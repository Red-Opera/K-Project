using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct VacationSetting{
    public int levelUpHP;
    public int levelUpDamage;
    public int levelUpDefense;
    public float additionalAttackSpeed;
    public float additionalCritical;
    public float additionalCriticalDamage;
}
public class MyVocation : MonoBehaviour
{
    public VacationSetting myVacation;
    void Start()
    {
        UpdateVacationState();
    }

    // Update is called once per frame
    void UpdateVacationState()
    {
        GameManager.info.addLevelState.currentHp = myVacation.levelUpHP;
        GameManager.info.addLevelState.maxHP = myVacation.levelUpHP;
        GameManager.info.addLevelState.damage = myVacation.levelUpDamage;
        GameManager.info.addLevelState.defense = myVacation.levelUpDefense;
        GameManager.info.playerState.attackSpeed = myVacation.additionalAttackSpeed;
        GameManager.info.playerState.critical = myVacation.additionalCritical;
        GameManager.info.playerState.criticalDamage = myVacation.additionalCriticalDamage;
    }
}
