using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject explosionPrefab;  // 生成するプレファブ
    public float spawnInterval = 1f;    // 生成の間隔

    private float timer = 0f;

    void Update()
    {
        // タイマーの更新
        timer += Time.deltaTime;

        // 一定の間隔でプレファブを生成
        if (timer >= spawnInterval)
        {
            SpawnPrefab();
            timer = 0f;  // タイマーを停止
        }
    }

    void SpawnPrefab()
    {
        // プレファブを生成
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
