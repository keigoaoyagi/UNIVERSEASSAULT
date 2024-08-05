using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Text textToBlink;
    public Image fadePanel;             // �t�F�[�h�A�E�gImage
    public AudioSource audioSource;     // �I�[�f�B�I�\�[�X
    public AudioClip soundClip;         // �Đ����鉹

    public string nextSceneName;        // �ڍs��̃V�[����
    public float blinkDuration = 2.0f;  // �_�ł̌p������
    public float blinkInterval = 0.2f;  // �_�ł̃C���^�[�o��
    public float waitDuration = 5.0f;   // �ҋ@����
    public float fadeDuration = 2.0f;   // �t�F�[�h�A�E�g�̎���

    private bool isBlinking = false;
    private bool waitingForKey = true;  // �L�[���͑ҋ@�t���O
    private float blinkTimer = 0.0f;

    private void Start()
    {
        fadePanel.color = Color.black;  // �t�F�[�h�A�E�g�p�l�������ɏ�����
        StartFadeIn();                  // �A�j���[�V�������J�n
    }

    private void Update()
    {
        // �L�[�������ꂽ��_�ł��J�n
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

        // �ҋ@��Ƀt�F�[�h�A�E�g���J�n
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
