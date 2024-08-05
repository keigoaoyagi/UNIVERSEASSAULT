using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossZone : MonoBehaviour
{
    public Text BossText;                   // UIテキストを参照
    public Image BossHPBar;                 // UIImageの参照
    public GameObject bossObject;           // ボスのゲームオブジェクト
    private Player player;                  // プレイヤーオブジェクト参照
    private Boss boss;                      // ボススクリプト参照
    public Image fadeImage;                 // フェードのImage

    public float fadeSpeed = 1.5f;          // フェードの速さ
    private bool isFading = false;          // フェード中のフラグ


    private void Start()
    {
        player = FindObjectOfType<Player>();        // プレイヤーを検索
        boss = FindObjectOfType<Boss>();            // ボスを検索

        BossText.gameObject.SetActive(false);　     
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
        // フェードアウト
        yield return StartCoroutine(Fade(0f, 1f));

        if (bossObject != null)
        {
            // 全身処理を停止
            player.StopForwardMovement();

            // ボスを表示
            bossObject.SetActive(true);

            // 攻撃処理開始
            boss.StartBossProcess();

            // HPバーを表示
            BossText.gameObject.SetActive(true);
            BossHPBar.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("BossObject is not set!");
        }

        // フェードイン
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