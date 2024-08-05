using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource destructionSound; // 破壊時の効果音
    public AudioSource PlayerDeathSound; // プレイヤー死亡サウンド
    public AudioSource PlayerRollSound;  // プレイヤー回転サウンド
    public AudioSource EnemyHitSound;    // 敵ヒットサウンド
    public AudioSource ChargeSound;      // チャージサウンド
    public AudioSource ChargeShotSound;  // チャージショットサウンド

    void Start()
    {
        // サウンドがアタッチされているか確認
        if (destructionSound == null)
        {
            Debug.LogError("Destruction sound is not assigned to the AudioManager script.");
        }
    }

    public void PlayDestructionSound()
    {
        // 効果音を再生
        if (destructionSound != null)
        {
            destructionSound.Play();
        }
    }

    public void PlayDeathSound()
    {
        if (PlayerDeathSound != null)
        {
            PlayerDeathSound.Play();
        }
    }

    public void rollSound()
    {
        if (PlayerRollSound != null)
        {
            PlayerRollSound.Play();
        }
    }

    public void enemyHitSound()
    {
        if (EnemyHitSound != null)
        {
            EnemyHitSound.Play();
        }

    }

    public void ChargeingSound()
    {
        if (ChargeSound != null)
        {
            ChargeSound.Play();
        }
    }

    public void ChargeShot()
    {
        if (ChargeShotSound != null)
        {
            ChargeShotSound.Play();
        }
    }

}
