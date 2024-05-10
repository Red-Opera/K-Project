using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFade : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    public float fadeTime;
    private Image image;

    private void OnEnable()
    {
        image = GetComponent<Image>();

        StartCoroutine(Fade(1, 0));

    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent <= 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color =image.color;
            color.a = Mathf.Lerp(start,end,percent);
            image.color = color;

            yield return null;
        }
    }
}
