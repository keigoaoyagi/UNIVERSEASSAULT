using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    public Image barImage;              // �o�[�̃R���|�[�l���g

    private float currentHealth;        // ���݂�HP
    private float maxHealth = 100f;     // �ő�HP
    private float targetHealth;         // �ڕW��HP
    private float smoothSpeed = 1.5f;   // �ω��̑��x
    private bool isAnimating = false;   // �A�j���[�V���������ǂ���
    private float vibrationMagnitude = 5.0f; // �U���̋���

    void Start()
    {
        currentHealth = 0f;             // �����l��0�ɐݒ�
        targetHealth = maxHealth;       // �ڕW��HP���ő�l�ɐݒ�
        UpdateBar();                    // �����̃o�[��Ԃ��X�V
        StartAnimation();               // �A�j���[�V�����J�n
    }

    void Update()
    {
        // �X���[�Y��HP�o�[�̕ω�
        SmoothHPChange();
    }

    private void StartAnimation()
    {
        isAnimating = true;
        StartCoroutine(StartVibration());
        StartCoroutine(AnimateToMaxHealth());
    }

    // HP�ɉ����ăT�C�Y�𒲐�
    private void UpdateBar()
    {
        float fillAmount = currentHealth / maxHealth;   // HP�������v�Z
        barImage.fillAmount = fillAmount;               // Image��fillAmount��ݒ�
    }

    // �X���[�Y�ȃo�[�̕ω�����
    private void SmoothHPChange()
    {
        if (isAnimating)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, Time.deltaTime * smoothSpeed);

            // �o�[��FillAmount���X�V
            UpdateBar();
        }
    }

    // HP�����������鏈��
    public void TakeDamage(float damageAmount)
    {
        targetHealth -= damageAmount;

        // �_���[�W���󂯂���U���ƃA�j���[�V�������Đ�
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(StartVibration());
            StartCoroutine(AnimateToMaxHealth());
        }
    }

    // HP���񕜂����鏈��
    public void Heal(float healAmount)
    {
        targetHealth += healAmount;

        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(StartVibration());
            StartCoroutine(AnimateToMaxHealth());
        }
    }

    // �U�����J�n����R���[�`��
    private IEnumerator StartVibration()
    {
        float elapsed = 0f;
        Vector3 originalPosition = transform.position;

        while (elapsed < 0.5f)  
        {
            float offsetX = Random.Range(-vibrationMagnitude, vibrationMagnitude);
            float offsetY = Random.Range(-vibrationMagnitude, vibrationMagnitude);
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // �U���I�����Ɍ��̈ʒu�ɖ߂�
        transform.position = originalPosition;
        isAnimating = false;
    }

    // �ő�HP�܂ł̃A�j���[�V����
    private IEnumerator AnimateToMaxHealth()
    {
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, elapsed);
            UpdateBar();
            elapsed += Time.deltaTime;
            yield return null;
        }

        // �A�j���[�V�����I�����ɐU�����~
        isAnimating = false;
    }
}
