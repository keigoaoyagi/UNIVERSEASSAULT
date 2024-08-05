using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource destructionSound; // �j�󎞂̌��ʉ�
    public AudioSource PlayerDeathSound; // �v���C���[���S�T�E���h
    public AudioSource PlayerRollSound;  // �v���C���[��]�T�E���h
    public AudioSource EnemyHitSound;    // �G�q�b�g�T�E���h
    public AudioSource ChargeSound;      // �`���[�W�T�E���h
    public AudioSource ChargeShotSound;  // �`���[�W�V���b�g�T�E���h

    void Start()
    {
        // �T�E���h���A�^�b�`����Ă��邩�m�F
        if (destructionSound == null)
        {
            Debug.LogError("Destruction sound is not assigned to the AudioManager script.");
        }
    }

    public void PlayDestructionSound()
    {
        // ���ʉ����Đ�
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
