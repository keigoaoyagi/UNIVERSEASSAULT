using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public Material flashMaterial;          // �_�Ŏ��̃}�e���A��

    private Material originalMaterial;      // ���̃}�e���A��
    private Renderer renderer;              // �����_���[�R���|�[�l���g
    public GameObject explosionPrefab;      // �����̃v���t�@�u
    public AudioManager audioManager;       

    public float rotationSpeedY = 20f;      // Y���̉�]���x
    public float rotationSpeedZ = 30f;      // Z���̉�]���x
    public float flashDuration = 0.01f;     // �}�e���A���̓_�Ŏ���
    public int collisionCount = 0;          // �Փˉ񐔂̃J�E���g
    public int maxCollisionCount = 3;       // �ő�Փˉ�
    public float explosionDelay = 0.05f;    // �����̒x�����ԁi�b�j

    private bool isFlashing = false;        // �_�Œ��̃t���O

    private float randomRotationSpeedX;     // �����_����X�̉�]���x

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material; // ���̃}�e���A����ۑ�

        randomRotationSpeedX = Random.Range(-rotationSpeedY, rotationSpeedY); // �����_����X���̉�]�̏�����
    }

    // Update is called once per frame
    void Update()
    {
        // X���̃����_���ȉ�]
        transform.Rotate(Vector3.right * randomRotationSpeedX * Time.deltaTime);

        // Y���̉�]
        transform.Rotate(Vector3.up * rotationSpeedY * Time.deltaTime);

        // Z���̉�]
        transform.Rotate(Vector3.forward * rotationSpeedZ * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            // Wall�^�O�̃I�u�W�F�N�g�̏ꍇ�̓X�L�b�v
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
            // ��莞�Ԍ�ɔ����𐶐�
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

        // �t���b�V���}�e���A����K�p
        renderer.material = flashMaterial;

        yield return new WaitForSeconds(flashDuration);

        // ���̃}�e���A���ɖ߂�
        renderer.material = originalMaterial;

        isFlashing = false;
    }

    private void Explode()
    {
        audioManager.PlayDestructionSound();

        // �����̃v���t�@�u�𐶐�
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        // �p�[�e�B�N���V�X�e���̎擾
        ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();

        // �p�[�e�B�N�����Đ������܂őҋ@
        float waitTime = explosionParticles.main.duration;
        Destroy(explosion, waitTime);

        // �I�u�W�F�N�g��j��
        Destroy(gameObject);
    }

}
