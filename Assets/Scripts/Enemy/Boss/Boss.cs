using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Boss : MonoBehaviour
{
    public Transform target;                // 追尾対象のトランスフォーム
    public Transform firePoint1;            // 弾を発射する位置
    public Transform firePoint2;            
    public GameObject bulletPrefab;         // 弾のプレファブ
    public ParticleSystem particleSystem;   // 弾を再生するパーティクルシステム
    public Text scoreText;                  // スコアを表示するUIテキスト
    public BossHP hpBar;                    // ボスのHPバー参照
    public Material flashMaterial;          // 点滅時のマテリアル
    private Material originalMaterial;      // 元のマテリアル
    private Quaternion initialRotation;     // 初期の回転値
    private Renderer renderer;              // レンダラーコンポーネント

    public float moveSpeed = 3f;            // ボスの移動速度
    public float attackInterval = 2f;       // 攻撃の間隔
    public float bulletSpeed = 10f;         // 弾の速度
    public float particleDuration = 3f;     // パーティクルの再生時間
    public float playDuration = 0f;
    public int maxCollisionCount = 5;       // 最大衝突回数


    private float attackTimer = 0f;         // 攻撃のタイマー
    private float playTime = 0f;
    private bool isFlashing = false;        // 点滅時のフラグ
    private bool isFireing = false;         // 攻撃のフラグ
    private bool isRotationLocked = false;  // 回転固定フラグ
    private bool isPlaying = false;         // パーティクル再生時のフラグ
    private int collisionCount = 0;         // 最大衝突回数のカウント

    public string NextSceneName;            // 移行先のシーンの指定

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material; // 元のマテリアルを保存
    }

    void Update()
    {
        if (isRotationLocked == false)
        {
            Tracking();
        }

        if (isFireing == true)
        {
            if (target == null)
                return;

            // 弾を発射するタイミングを管理
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                // ランダムな値を生成
                float randomValue = Random.Range(0, 3);

                if (randomValue == 0)
                {
                    // 弾を発射
                    Attack();
                }
                else if (randomValue == 1)
                {
                    isRotationLocked = true;
                    initialRotation = transform.rotation; // 初期の回転値を保存
                    transform.rotation = initialRotation; // オブジェクトの回転値を初期の回転地に固定

                    if (isRotationLocked)
                    {
                        // パーティクルを再生
                        Invoke("PlayParticles",2);
                    }
                }
                else if(randomValue == 2)
                {

                }

                attackTimer = 0f; // タイマーをリセット
            }

            if (isPlaying)
            {
                // 経過時間を計算
                playTime += Time.deltaTime;

                // 再生時間に達したら再生を停止
                if (playTime >= playDuration)
                {
                    Stop();
                }
            }
        }
    }

    private void Attack()
    {


        // 弾のプレファブから弾オブジェクトを生成
        GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);

        // 弾の速度を設定
        Rigidbody bulletRigidbody1 = bullet1.GetComponent<Rigidbody>();
        Vector3 bulletDirection1 = (target.position - firePoint1.position).normalized;
        bulletRigidbody1.velocity = bulletDirection1 * bulletSpeed; 

        Rigidbody bulletRigidbody2 = bullet2.GetComponent<Rigidbody>();
        Vector3 bulletDirection2 = (target.position - firePoint2.position).normalized;
        bulletRigidbody2.velocity = bulletDirection2 * bulletSpeed;

        // 一定時間後に弾を破壊する
        Destroy(bullet1, 5f);
        Destroy(bullet2, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chargebullet"))
        {
            collisionCount += 10;
            // HPバーを減少させる
            hpBar.TakeDamage(10f);
        }
        else
        {
            collisionCount++;
            // HPバーを減少させる
            hpBar.TakeDamage(1f);
        }


        // UIテキストのスコアを増やす
        IncreaseScore(1);

        // 指定回数の衝突があったら自身を破壊
        if (collisionCount >= maxCollisionCount)
        {

            LoadNextScene();

            Destroy(gameObject);
        }
        else
        {
            if (!isFlashing)
            {
                StartCoroutine(FlashMaterial());
            }
        }
    }

    private IEnumerator FlashMaterial()
    {
        isFlashing = true;

        // フラッシュマテリアルを適用
        renderer.material = flashMaterial;

        // フラッシュの時間
        yield return new WaitForSeconds(0.2f);

        // 元のマテリアルに戻す
        renderer.material = originalMaterial;

        isFlashing = false;
    }

    private void IncreaseScore(int amount)
    {
        int currentScore = int.Parse(scoreText.text);
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    public void StartBossProcess()
    {
        isFireing = true;
    }

    private void PlayParticles()
    {
        isPlaying = true;

        // パーティクルを再生
        particleSystem.Play();
    }

    private void Stop()
    {
        particleSystem.Stop();
        isRotationLocked = false;
        isPlaying = false;
        playTime = 0f;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }

    private void Tracking()
    {
        if (isPlaying != true)
        {
            //回転固定が解除された場合、オブジェクトの回転を更新
            Vector3 targetDirection = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }

    }
}