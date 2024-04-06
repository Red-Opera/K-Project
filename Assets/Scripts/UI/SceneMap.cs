using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMap : MonoBehaviour
{
    [SerializeField] private RawImage mapImage;

    public void Start()
    {
        SceneManager.sceneLoaded += SceneLoad;
    }

    private void SceneLoad(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Map")
            mapImage.uvRect.size.Set(1.0f, 0.5f);
    }

    public void Update()
    {
        
    }
}
