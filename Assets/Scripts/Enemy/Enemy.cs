using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float projectileSpeed = 10f;     // 弾の速度
    public float fireRate = 2f;             // 発射間隔
    public float projectileLifetime = 5f;   // 弾の生存時間
    public float flashDuration = 1f;        // マテリアルの点滅時間
    public float movementRangeX = 10f;      // x方向の移動範囲
    public float movementRangeY = 10f;      // y方向の移動範囲
    public float movementSpeed = 5f;        // 移動範囲
    public float explosionDelay = 0.05f;    // 爆発の遅延時間（秒）


    private bool isFlashing = false;        // 点滅中のフラグ
    private bool isFiring;                  // 発射中のフラグ
    private bool isShaking = false;
    private int score = 0;                  // スコアの初期値
    private int collisionCount;             // 衝突回数のカウント
    private float fireTimer;                // 発射タイマー
    public float ShakeDuration1 = -15;
    public float ShakeDuration2 = 15;


    private bool isInPlayerRange = false;   // プレイヤー検出時のフラグ

    private Vector3 startPosition;          // 初期位置
    private Vector3 targetPosition;         // 目標位置
    public  GameObject projectilePrefab;    // 弾のプレファブ
    public  Transform target;               // 発射対象のゲームオブジェクト
    public  Transform firePoint;            // 弾を発射する位置
    public  Material flashMaterial;         // 点滅時のマテリアル
    private Material originalMaterial;      // 元のマテリアル
    private Renderer renderer;              // レンダラーコンポーネント
    public GameObject explosionPrefab;      // 爆発のプレファブ
    public AudioManager audioManager;       // AudioManagerの参照


    public Text scoreText;                  // スコアのUIを参照

    [SerializeField]
    private float DestroyCount = 5f;
    [SerializeField]
    private float PlayerDistance = 200f;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material; // 元のマテリアルを保存
        startPosition = transform.position;
        targetPosition = GetRandomPosition();
    }

    void Update()
    {
        if (!isInPlayerRange)
            return;

        if (target == null)
            return;

        // 移動処理
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            targetPosition = GetRandomPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        // 発射タイマーを開始
        fireTimer += Time.deltaTime;

        // 発射間隔に達したら弾を発射
        if (fireTimer >= 1f / fireRate)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    private void FireProjectile()
    {
        // 弾のプレファブから弾オブジェクトを生成
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 弾の方向を計算
        Vector3 direction = (target.position - firePoint.position).normalized;

        // 弾の速度を設定
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = direction * projectileSpeed;

        // 一定時間後に弾を破壊する
        Destroy(projectile, projectileLifetime);

        isFiring = true;
    }

    private void StopFiring()
    {
        if (isFiring)
        {
            isFiring = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.gameObject.CompareTag("ebullet"))
        {
            // タグがebulletの場合は処理をスキップ
            return;
        }

        if(other.gameObject.CompareTag("Chargebullet"))
        {
            collisionCount += 3;
        }
        else
        {
            collisionCount++;
        }

        // UIテキストのスコアを増やす
        IncreaseScore(1);
        if (collisionCount >= DestroyCount)
        {
            // 一定時間後に爆発を生成
            Invoke("Explode", explosionDelay);
        }
        else
        {
            StartCoroutine(FlashMaterial());
        }
    }

    IEnumerator FlashMaterial()
    {
        isFlashing = true;

        // フラッシュマテリアルを適用
        renderer.material = flashMaterial;

        // 0.5秒間敵を揺らす
        StartCoroutine(ShakeEnemy(0.5f));

        yield return new WaitForSeconds(flashDuration);

        // 元のマテリアルに戻す
        renderer.material = originalMaterial;

        isFlashing = false;
    }

    private void UpdatePlayerRange()
    {
        if (target == null)
            return;

        // プレイヤーと敵の距離を計算
        float distance = Vector3.Distance(target.position, transform.position);

        // 距離が一定範囲以内なら攻撃
        if (distance <= PlayerDistance)
        {
            isInPlayerRange = true;
        }
        else
        {
            isInPlayerRange = false;
            StopFiring();
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(startPosition.x - movementRangeX, startPosition.x + movementRangeX);
        float randomY = Random.Range(startPosition.y - movementRangeY, startPosition.y + movementRangeY);

        return new Vector3(randomX, randomY, transform.position.z);
    }

    private void LateUpdate()
    {
        UpdatePlayerRange();
    }

    // スコアを更新
    private void IncreaseScore(int amount)
    {
        int currentScore = int.Parse(scoreText.text);
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    private void Explode()
    {

        audioManager.PlayDestructionSound();

        // 爆発のプレファブを生成
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        // パーティクルシステムを取得
        ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();

        // パーティクルが再生されるまで待機
        float waitTime = explosionParticles.main.duration;
        Destroy(explosion, waitTime);

        // オブジェクトを破壊
        Destroy(gameObject);
    }

    IEnumerator ShakeEnemy(float duration)
    {
        if (!isShaking)
        {
            isShaking = true;

            Quaternion originalRotation = transform.localRotation;
            float elapsed = 0f;

            audioManager.enemyHitSound();

            while (elapsed < duration)
            {
                float pitch = Random.Range(0f, 0f);
                float yaw = Random.Range(0f, 0f);
                float roll = Random.Range(ShakeDuration1, ShakeDuration2);

                // 元の回転に新しい回転を加える
                Quaternion newRotation = originalRotation * Quaternion.Euler(pitch, yaw, roll);
                transform.localRotation = newRotation;

                elapsed += Time.unscaledDeltaTime;

                yield return null;
            }

            // 元の回転に戻す
            transform.localRotation = originalRotation;
            isShaking = false;
        }
    }

}
