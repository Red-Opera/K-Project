using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FadeEffect : MonoBehaviour
{  
    [SerializeField]
    [Range(0.01f, 10f)]
    private float  fadeTime;
    private Image image;
   
    private void Awake() 
    {
        image = GetComponent<Image>();
        StartCoroutine(Fade(1,0));
    }
    private IEnumerator Fade(float start,float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        Color color = image.color;

        while (currentTime / fadeTime < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            color.a =Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;

        }

        color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    }
}
