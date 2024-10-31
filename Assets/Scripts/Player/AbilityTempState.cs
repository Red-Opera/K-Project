using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Scriptable/PlayerState", order = int.MaxValue)]
public class AbilityTempState : ScriptableObject
{
    public string nickName;             // 플레이어 닉네임
    public int Anger;
    public int Haste;
    public int Patience;
    public int Mystery;
    public int Greed;
    public int Craving;
}