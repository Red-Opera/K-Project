using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] string nextScene;
    
    void Update()
    {
        if (Input.anyKeyDown)
            SceneManager.LoadScene(nextScene);
    }
}
