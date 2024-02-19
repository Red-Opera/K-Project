using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Scriptable/Food", order = int.MaxValue)]
public class FoodState : ScriptableObject
{
    [System.Serializable]
    public class Info
    {
        public string stateName;
        public float value;
    }

    public string foodName;
    public List<Info> addState;
    public List<Info> baseState;
    public bool isHotFood;
}