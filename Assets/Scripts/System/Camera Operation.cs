using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;               // 追従するプレイヤーオブジェクト
    public Vector3 initialPosition;         // アニメーションの開始位置

    private AudioSource audioSource;        // オーディオソースコンポーネント

    public float cameraZOffset = -10f;      // カメラのZ軸オフセット
    public float animationDuration = 3.0f;  // アニメーションの時間

    private float animationTimer = 0.0f;    // アニメーションの経過時間

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;                        // ループ再生を有効
        audioSource.Play();                             // BGMを有効
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // アニメーションの経過時間を更新
            animationTimer += Time.deltaTime;

      　　　// アニメーションが終了していない場合
            if (animationTimer < animationDuration)
            {
                // アニメーションと同期してカメラの位置を更新
                float t = animationTimer / animationDuration;
                Vector3 newPosition = Vector3.Lerp(initialPosition, player.transform.position, t);
                newPosition.z += cameraZOffset; // Zオフセット適用
                transform.position = newPosition;
            }
            else
            {
                // アニメーション終了時にカメラをプレイヤーに固定
                Vector3 newPosition = transform.position;
                newPosition.z = player.transform.position.z + cameraZOffset;
                transform.position = newPosition;
            }
        }
    }
}