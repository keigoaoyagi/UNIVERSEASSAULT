using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    public Image barImage;              // バーのコンポーネント

    private float currentHealth;        // 現在のHP
    private float maxHealth = 100f;     // 最大HP
    private float targetHealth;         // 目標のHP
    private float smoothSpeed = 1.5f;   // 変化の速度
    private bool isAnimating = false;   // アニメーション中かどうか
    private float vibrationMagnitude = 5.0f; // 振動の強さ

    void Start()
    {
        currentHealth = 0f;             // 初期値を0に設定
        targetHealth = maxHealth;       // 目標のHPを最大値に設定
        UpdateBar();                    // 初期のバー状態を更新
        StartAnimation();               // アニメーション開始
    }

    void Update()
    {
        // スムーズなHPバーの変化
        SmoothHPChange();
    }

    private void StartAnimation()
    {
        isAnimating = true;
        StartCoroutine(StartVibration());
        StartCoroutine(AnimateToMaxHealth());
    }

    // HPに応じてサイズを調整
    private void UpdateBar()
    {
        float fillAmount = currentHealth / maxHealth;   // HP割合を計算
        barImage.fillAmount = fillAmount;               // ImageのfillAmountを設定
    }

    // スムーズなバーの変化処理
    private void SmoothHPChange()
    {
        if (isAnimating)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, Time.deltaTime * smoothSpeed);

            // バーのFillAmountを更新
            UpdateBar();
        }
    }

    // HPを減少させる処理
    public void TakeDamage(float damageAmount)
    {
        targetHealth -= damageAmount;

        // ダメージを受けたら振動とアニメーションを再生
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(StartVibration());
            StartCoroutine(AnimateToMaxHealth());
        }
    }

    // HPを回復させる処理
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

    // 振動を開始するコルーチン
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

        // 振動終了時に元の位置に戻す
        transform.position = originalPosition;
        isAnimating = false;
    }

    // 最大HPまでのアニメーション
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

        // アニメーション終了時に振動を停止
        isAnimating = false;
    }
}
