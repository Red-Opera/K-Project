using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Scriptable/PlayerState", order = int.MaxValue)]
public class AbilityTempState : ScriptableObject
{
    public string nickName;             // 플레이어 닉네임
    public int Anger;      //공격력
    public int Haste;      //공, 이속
    public int Patience;    //방어력
    public int Mystery;     //치명타, 회피
    public int Greed;       //최대 체력
    public int Craving;     //강인함?
    public float AEffectD; //anger 효과 - 공격력+
    public float AEffectH; //anger 효과 - 체력 -
    public float HEffect; //Haste 효과 - 확률로 2타
    public float PEffect; //Patience 효과 - 쉴드
    public float MEffect; // Mystery 효과 - 크리티컬 데미지
    public float GEffect; // Greed 효과 - 상점 할인 
    public float CEffect; // Craving 효과 - 블록
    
}