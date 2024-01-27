using System;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private State state;
    [SerializeField] private GameObject states;

    void Start()
    {
        Debug.Assert(state != null, "플레이어 상태가 없습니다.");
        Debug.Assert(states != null, "상태를 출력할 창이 없습니다.");
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        for (int i = 0; i < states.transform.childCount; i++)
        {
            string stateName = states.transform.GetChild(i).gameObject.name;

            object fieldValue = null;
            state.datas[stateName].GetValue(fieldValue);

            if (fieldValue == null)
                continue;
        }
    }
}
