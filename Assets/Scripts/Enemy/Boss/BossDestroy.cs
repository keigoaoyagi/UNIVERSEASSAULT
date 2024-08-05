using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDestroy : MonoBehaviour
{
    public Material blinkMaterial;          // 点滅時に使用するマテリアル
    public GameObject explosionPrefab;      // 爆発のプレファブ

    private Renderer renderer;              // オブジェクトのレンダラー
    private Material originalMaterial;      // 元のマテリアル

    public float blinkDuration = 2f;        // 点滅の期間（秒）
    public float explosionDelay = 3f;       // 爆発の遅延時間（秒）

    private bool isBlinking = false;        // 点滅中のフラグ
    private float blinkTimer = 0f;          // 点滅タイマー


    private void Start()
    {

        // オブジェクトのレンダラーを取得
        renderer = GetComponent<Renderer>();

        // オブジェクトの元のマテリアルを保存
        originalMaterial = renderer.material;

        // オブジェクトを点滅させる
        StartBlinking();

        // 一定時間後に爆発
        Invoke("Explode", explosionDelay);

    }

    private void Update()
    {
        if (isBlinking)
        {
            // 点滅のタイミングを計算
            blinkTimer += Time.deltaTime;

            // 点滅の周期に合わせてマテリアルを切り替える
            if (blinkTimer >= blinkDuration)
            {
                if (renderer.material == originalMaterial)
                {
                    renderer.material = blinkMaterial;
                }
                else
                {
                    renderer.material = originalMaterial;
                }

                blinkTimer = 0f; // タイマーをリセット
            }
        }

    }

    private void StartBlinking()
    {
        isBlinking = true;
        blinkTimer = 0f; // タイマーをリセット
    }

    public void StopBlinking()
    {
        isBlinking = false;
        renderer.material = originalMaterial; // マテリアルを元に戻す
    }

    private void Explode()
    {
       
        
        // 爆発のプレファブを生成
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // オブジェクトを破壊
        Destroy(gameObject);
    }
}

