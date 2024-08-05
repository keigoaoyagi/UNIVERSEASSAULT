using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Image fadePanel;             // フェードイン用のUI
    public float fadeDuration = 2.0f;   // フェードインの時間

    void Start()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1f); 
        float startAlpha = 1f;
        float endAlpha = 0f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float normalizedTime = timer / fadeDuration;
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(startAlpha, endAlpha, normalizedTime));
            timer += Time.deltaTime;
            yield return null;
        }

        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, endAlpha);
    }
}
