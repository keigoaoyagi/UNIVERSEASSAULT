using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSys : MonoBehaviour
{
    public GameObject player;               // プレイヤーオブジェクト
    public Image fadeImage;                 // フェードアウトに使用するImage

    public float fadeOutDuration = 1.0f;    // フェードアウトの時間
    public string NextSceneName;            // 移行先のシーン名

    private bool playerInactive = false;    // プレイヤー非表示中のフラグ
    private bool fading = false;            // フェードアウト中のフラグ

    private void Update()
    {
        if (player == null)
        {
            // オブジェクトがnullの場合スキップ
            return;
        }

        if (!player.activeInHierarchy && !playerInactive && !fading)
        {
            // プレイヤーが非アクティブになったら
            // フェードアウト処理を開始
            fading = true;
            StartCoroutine(FadeOutAndLoadScene());
            playerInactive = true; // フラグを設定
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        Color startColor = fadeImage.color;
        Color targetColor = startColor;
        targetColor.a = 1.0f;

        float timer = 0.0f;
        while (timer < fadeOutDuration)
        {
            float normalizedTime = timer / fadeOutDuration;
            fadeImage.color = Color.Lerp(startColor, targetColor, normalizedTime);

            timer += Time.deltaTime;
            yield return null;
        }

        // 次のシーンに移行
        SceneManager.LoadScene(NextSceneName);
    }
}
