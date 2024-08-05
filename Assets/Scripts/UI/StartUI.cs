using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public AudioSource audioSource;

    public Text missionStartText; // �~�b�V�����X�^�[�g��UI�e�L�X�g
    public float animationStartTime = 5.0f; // �A�j���[�V�����J�n����
    public float animationDuration = 2.0f; // �A�j���[�V�����̎���

    void Start()
    {
        missionStartText.gameObject.SetActive(false);

        StartCoroutine(WaitAndStartAnimation());
    }


    private IEnumerator WaitAndStartAnimation()
    {
        yield return new WaitForSeconds(animationStartTime);

        StartCoroutine(AnimateMissionStart());
    }


    private IEnumerator AnimateMissionStart()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        missionStartText.gameObject.SetActive(true);

        float timer = 0.0f;
        while (timer < animationDuration)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, timer / animationDuration);
            missionStartText.color = new Color(missionStartText.color.r, missionStartText.color.g, missionStartText.color.b, alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        timer = 0.0f;
        while (timer < animationDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, timer / animationDuration);
            missionStartText.color = new Color(missionStartText.color.r, missionStartText.color.g, missionStartText.color.b, alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        missionStartText.gameObject.SetActive(false);
    }

}
