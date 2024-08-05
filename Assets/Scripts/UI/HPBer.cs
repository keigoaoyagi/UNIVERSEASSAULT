using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public Image barImage;                      

    private float currentHealth;                
    private float maxHealth = 100f;             
    private float targetHealth;                 
    private float smoothSpeed = 0.005f;         
    private float vibrationMagnitude = 5.0f;    
    private bool isAnimating = false;           


    void Start()
    {
        currentHealth = 0f;             
        targetHealth = maxHealth;       
        UpdateBar();                    
        StartAnimation();               
    }

    void Update()
    {
        SmoothHPChange();
    }

    private void StartAnimation()
    {
        isAnimating = true;
        StartCoroutine(StartVibration());
        StartCoroutine(AnimateToMaxHealth());
    }

    private void UpdateBar()
    {
        float fillAmount = currentHealth / maxHealth;   // HP�������v�Z
        barImage.fillAmount = fillAmount;               // Image��fillAmount��ݒ�
    }

    private void SmoothHPChange()
    {
        if (isAnimating)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, Time.deltaTime * smoothSpeed);

            // �o�[��Fill Amount���X�V
            UpdateBar();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        targetHealth -= damageAmount;

        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(StartVibration());
            StartCoroutine(AnimateToMaxHealth());
        }
    }

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

    private IEnumerator StartVibration()
    {
        float elapsed = 0f;
        Vector3 originalPosition = transform.position;

        while (elapsed < 0.5f)  // 0.5�b�ԐU��
        {
            float offsetX = Random.Range(-vibrationMagnitude, vibrationMagnitude);
            float offsetY = Random.Range(-vibrationMagnitude, vibrationMagnitude);
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isAnimating = false;
    }

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

        isAnimating = false;
    }
}
