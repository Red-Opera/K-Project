using System;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private State state;
    [SerializeField] private GameObject states;

    void Start()
    {
        Debug.Assert(state != null, "�÷��̾� ���°� �����ϴ�.");
        Debug.Assert(states != null, "���¸� ����� â�� �����ϴ�.");
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
