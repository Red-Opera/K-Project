using System.Collections.Generic;
using UnityEngine;

public class MainTheadAction : MonoBehaviour
{
    private static readonly Queue<System.Action> actions = new Queue<System.Action>();

    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
                actions.Dequeue().Invoke();
        }
    }

    public static void Enqueue(System.Action action)
    {
        lock(actions)
            actions.Enqueue(action);
    }
}
