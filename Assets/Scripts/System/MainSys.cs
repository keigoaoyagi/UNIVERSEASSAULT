using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSys : MonoBehaviour
{
    public GameObject player;               // �v���C���[�I�u�W�F�N�g
    public Image fadeImage;                 // �t�F�[�h�A�E�g�Ɏg�p����Image

    public float fadeOutDuration = 1.0f;    // �t�F�[�h�A�E�g�̎���
    public string NextSceneName;            // �ڍs��̃V�[����

    private bool playerInactive = false;    // �v���C���[��\�����̃t���O
    private bool fading = false;            // �t�F�[�h�A�E�g���̃t���O

    private void Update()
    {
        if (player == null)
        {
            // �I�u�W�F�N�g��null�̏ꍇ�X�L�b�v
            return;
        }

        if (!player.activeInHierarchy && !playerInactive && !fading)
        {
            // �v���C���[����A�N�e�B�u�ɂȂ�����
            // �t�F�[�h�A�E�g�������J�n
            fading = true;
            StartCoroutine(FadeOutAndLoadScene());
            playerInactive = true; // �t���O��ݒ�
        }
    }

    private IEnumerator FadeOutAndLoadScene()
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

        // ���̃V�[���Ɉڍs
        SceneManager.LoadScene(NextSceneName);
    }
}
