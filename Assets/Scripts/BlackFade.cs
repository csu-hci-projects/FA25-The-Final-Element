using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BlackFade : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image fadeImage;

    private void Awake()
    {
        // make sure it starts transparent
        Color c = fadeImage.color;
        c.a = 0;
        fadeImage.color = c;
    }

    public IEnumerator FadeToBlack(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / duration);
            Color c = fadeImage.color;
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }
    }
}
