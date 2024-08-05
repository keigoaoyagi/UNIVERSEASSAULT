using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public Material flashMaterial;          // 点滅時のマテリアル

    private Material originalMaterial;      // 元のマテリアル
    private Renderer renderer;              // レンダラーコンポーネント
    public GameObject explosionPrefab;      // 爆発のプレファブ
    public AudioManager audioManager;       

    public float rotationSpeedY = 20f;      // Y軸の回転速度
    public float rotationSpeedZ = 30f;      // Z軸の回転速度
    public float flashDuration = 0.01f;     // マテリアルの点滅時間
    public int collisionCount = 0;          // 衝突回数のカウント
    public int maxCollisionCount = 3;       // 最大衝突回数
    public float explosionDelay = 0.05f;    // 爆発の遅延時間（秒）

    private bool isFlashing = false;        // 点滅中のフラグ

    private float randomRotationSpeedX;     // ランダムなXの回転速度

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material; // 元のマテリアルを保存

        randomRotationSpeedX = Random.Range(-rotationSpeedY, rotationSpeedY); // ランダムなX軸の回転の初期化
    }

    // Update is called once per frame
    void Update()
    {
        // X軸のランダムな回転
        transform.Rotate(Vector3.right * randomRotationSpeedX * Time.deltaTime);

        // Y軸の回転
        transform.Rotate(Vector3.up * rotationSpeedY * Time.deltaTime);

        // Z軸の回転
        transform.Rotate(Vector3.forward * rotationSpeedZ * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            // Wallタグのオブジェクトの場合はスキップ
            return;
        }

        if (other.gameObject.CompareTag("Chargebullet"))
        {
            collisionCount += 6;
        }
        else
        {
            collisionCount++;
        }

        if (collisionCount >= maxCollisionCount)
        {
            // 一定時間後に爆発を生成
            Invoke("Explode", explosionDelay);
        }
        else
        {
            if (!isFlashing)
            {
                audioManager.enemyHitSound();
                StartCoroutine(FlashMaterial());
            }
        }
    }

    IEnumerator FlashMaterial()
    {
        isFlashing = true;

        // フラッシュマテリアルを適用
        renderer.material = flashMaterial;

        yield return new WaitForSeconds(flashDuration);

        // 元のマテリアルに戻す
        renderer.material = originalMaterial;

        isFlashing = false;
    }

    private void Explode()
    {
        audioManager.PlayDestructionSound();

        // 爆発のプレファブを生成
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        // パーティクルシステムの取得
        ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();

        // パーティクルが再生されるまで待機
        float waitTime = explosionParticles.main.duration;
        Destroy(explosion, waitTime);

        // オブジェクトを破壊
        Destroy(gameObject);
    }

}
