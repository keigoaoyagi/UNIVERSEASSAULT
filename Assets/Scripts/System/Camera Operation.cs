using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;               // �Ǐ]����v���C���[�I�u�W�F�N�g
    public Vector3 initialPosition;         // �A�j���[�V�����̊J�n�ʒu

    private AudioSource audioSource;        // �I�[�f�B�I�\�[�X�R���|�[�l���g

    public float cameraZOffset = -10f;      // �J������Z���I�t�Z�b�g
    public float animationDuration = 3.0f;  // �A�j���[�V�����̎���

    private float animationTimer = 0.0f;    // �A�j���[�V�����̌o�ߎ���

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;                        // ���[�v�Đ���L��
        audioSource.Play();                             // BGM��L��
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // �A�j���[�V�����̌o�ߎ��Ԃ��X�V
            animationTimer += Time.deltaTime;

      �@�@�@// �A�j���[�V�������I�����Ă��Ȃ��ꍇ
            if (animationTimer < animationDuration)
            {
                // �A�j���[�V�����Ɠ������ăJ�����̈ʒu���X�V
                float t = animationTimer / animationDuration;
                Vector3 newPosition = Vector3.Lerp(initialPosition, player.transform.position, t);
                newPosition.z += cameraZOffset; // Z�I�t�Z�b�g�K�p
                transform.position = newPosition;
            }
            else
            {
                // �A�j���[�V�����I�����ɃJ�������v���C���[�ɌŒ�
                Vector3 newPosition = transform.position;
                newPosition.z = player.transform.position.z + cameraZOffset;
                transform.position = newPosition;
            }
        }
    }
}