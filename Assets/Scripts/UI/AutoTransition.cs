using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoTransition: MonoBehaviour
{
    public Image fadeImage;                     

    private Coroutine currentFadeCoroutine;

    public float fadeDuration = 1.0f;           
    public float fadeInDuration = 1.0f;         
    public float fadeOutDuration = 1.0f;        
    public float delayBeforeFadeOut = 2.0f;     
    public string NextSceneName;                


    void Start()
    {
        currentFadeCoroutine = StartCoroutine(StartWithFadeIn());
    }

    private IEnumerator StartWithFadeIn()
    {
        Color startColor = fadeImage.color;
        Color targetColor = startColor;
        targetColor.a = 0.0f;

        float timer = 0.0f;
        while (timer < fadeInDuration)
        {
            float normalizedTime = timer / fadeInDuration;
            fadeImage.color = Color.Lerp(startColor, targetColor, normalizedTime);

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeFadeOut);

        currentFadeCoroutine = StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        Color startColor = fadeImage.color;
        Color targetColor = startColor;
        targetColor.a = 1.0f;

        float timer = 0.0f;
        while (timer < fadeOutDuration)
        {
            float normalizedTime = timer / fadeOutDuration;
            fadeImage.color = Color.Lerp(startColor, targetColor, normalizedTime);

            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(NextSceneName);
    }
}
