using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossZone : MonoBehaviour
{
    public Text BossText;                   // UI�e�L�X�g���Q��
    public Image BossHPBar;                 // UIImage�̎Q��
    public GameObject bossObject;           // �{�X�̃Q�[���I�u�W�F�N�g
    private Player player;                  // �v���C���[�I�u�W�F�N�g�Q��
    private Boss boss;                      // �{�X�X�N���v�g�Q��
    public Image fadeImage;                 // �t�F�[�h��Image

    public float fadeSpeed = 1.5f;          // �t�F�[�h�̑���
    private bool isFading = false;          // �t�F�[�h���̃t���O


    private void Start()
    {
        player = FindObjectOfType<Player>();        // �v���C���[������
        boss = FindObjectOfType<Boss>();            // �{�X������

        BossText.gameObject.SetActive(false);�@     
        BossHPBar.gameObject.SetActive(false);      
        bossObject.SetActive(false);                

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnBoss());
        }
    }

    IEnumerator SpawnBoss()
    {
        // �t�F�[�h�A�E�g
        yield return StartCoroutine(Fade(0f, 1f));

        if (bossObject != null)
        {
            // �S�g�������~
            player.StopForwardMovement();

            // �{�X��\��
            bossObject.SetActive(true);

            // �U�������J�n
            boss.StartBossProcess();

            // HP�o�[��\��
            BossText.gameObject.SetActive(true);
            BossHPBar.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("BossObject is not set!");
        }

        // �t�F�[�h�C��
        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        if (!isFading)
        {
            isFading = true;

            Color color = fadeImage.color;
            float alpha = startAlpha;

            while (!Mathf.Approximately(alpha, targetAlpha))
            {
                alpha = Mathf.MoveTowards(alpha, targetAlpha, fadeSpeed * Time.deltaTime);
                color.a = alpha;
                fadeImage.color = color;

                yield return null;
            }

            isFading = false;
        }
    }
}