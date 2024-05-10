using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 1f;

    public bool isFadeOut = false;

    public IEnumerator FadeOut()
    {
        isFadeOut = true;

        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeDuration;
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, alpha);
            yield return null;
        }

        isFadeOut = false;
    }

    public IEnumerator FadeIn()
    {
        isFadeOut = true;
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, alpha);

            yield return null;
        }
        
        isFadeOut = false;
    }
}
