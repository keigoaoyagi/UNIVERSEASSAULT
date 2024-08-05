using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Text textToBlink;
    public Image fadePanel;             // フェードアウトImage
    public AudioSource audioSource;     // オーディオソース
    public AudioClip soundClip;         // 再生する音

    public string nextSceneName;        // 移行先のシーン名
    public float blinkDuration = 2.0f;  // 点滅の継続時間
    public float blinkInterval = 0.2f;  // 点滅のインターバル
    public float waitDuration = 5.0f;   // 待機時間
    public float fadeDuration = 2.0f;   // フェードアウトの時間

    private bool isBlinking = false;
    private bool waitingForKey = true;  // キー入力待機フラグ
    private float blinkTimer = 0.0f;

    private void Start()
    {
        fadePanel.color = Color.black;  // フェードアウトパネルを黒に初期化
        StartFadeIn();                  // アニメーションを開始
    }

    private void Update()
    {
        // キーが押されたら点滅を開始
        if (waitingForKey && Input.anyKeyDown)
        {
            waitingForKey = false;
            StartBlink();
            PlaySound();
        }

        if (isBlinking)
        {
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= blinkDuration)
            {
                StopBlink();
            }
            else
            {
                float remainder = blinkTimer % (2 * blinkInterval);
                if (remainder < blinkInterval)
                {
                    textToBlink.gameObject.SetActive(true);
                }
                else
                {
                    textToBlink.gameObject.SetActive(false);
                }
            }
        }
    }

    private void TransitionToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(waitDuration);

        // 待機後にフェードアウトを開始
        StartFadeOut(); 
    }

    private void StartBlink()
    {
        isBlinking = true;
        blinkTimer = 0.0f;
        textToBlink.gameObject.SetActive(true);

        StartCoroutine(WaitAndLoadNextScene());
    }

    private void StopBlink()
    {
        isBlinking = false;
        textToBlink.gameObject.SetActive(true);
    }

    private void StartFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
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

    private IEnumerator FadeOutCoroutine()
    {
        float startAlpha = 0f;
        float endAlpha = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float normalizedTime = timer / fadeDuration;
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(startAlpha, endAlpha, normalizedTime));
            timer += Time.deltaTime;
            yield return null;
        }

        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, endAlpha);

        TransitionToNextScene();
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(soundClip); 
    }
}
