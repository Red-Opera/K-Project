using UnityEngine;

public class UINotDestroyOpen : MonoBehaviour
{
    public static GameObject inventory;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
