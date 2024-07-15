using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject loginObject;
    
    private void Update()
    {
        if (Input.anyKeyDown && !loginObject.activeSelf)
        {
            loginObject.SetActive(true);
            Login.GameLogin();
        }
    }
}
