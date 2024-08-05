using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float projectileSpeed = 10f;     // �e�̑��x
    public float fireRate = 2f;             // ���ˊԊu
    public float projectileLifetime = 5f;   // �e�̐�������
    public float flashDuration = 1f;        // �}�e���A���̓_�Ŏ���
    public float movementRangeX = 10f;      // x�����̈ړ��͈�
    public float movementRangeY = 10f;      // y�����̈ړ��͈�
    public float movementSpeed = 5f;        // �ړ��͈�
    public float explosionDelay = 0.05f;    // �����̒x�����ԁi�b�j


    private bool isFlashing = false;        // �_�Œ��̃t���O
    private bool isFiring;                  // ���˒��̃t���O
    private bool isShaking = false;
    private int score = 0;                  // �X�R�A�̏����l
    private int collisionCount;             // �Փˉ񐔂̃J�E���g
    private float fireTimer;                // ���˃^�C�}�[
    public float ShakeDuration1 = -15;
    public float ShakeDuration2 = 15;


    private bool isInPlayerRange = false;   // �v���C���[���o���̃t���O

    private Vector3 startPosition;          // �����ʒu
    private Vector3 targetPosition;         // �ڕW�ʒu
    public  GameObject projectilePrefab;    // �e�̃v���t�@�u
    public  Transform target;               // ���ˑΏۂ̃Q�[���I�u�W�F�N�g
    public  Transform firePoint;            // �e�𔭎˂���ʒu
    public  Material flashMaterial;         // �_�Ŏ��̃}�e���A��
    private Material originalMaterial;      // ���̃}�e���A��
    private Renderer renderer;              // �����_���[�R���|�[�l���g
    public GameObject explosionPrefab;      // �����̃v���t�@�u
    public AudioManager audioManager;       // AudioManager�̎Q��


    public Text scoreText;                  // �X�R�A��UI���Q��

    [SerializeField]
    private float DestroyCount = 5f;
    [SerializeField]
    private float PlayerDistance = 200f;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material; // ���̃}�e���A����ۑ�
        startPosition = transform.position;
        targetPosition = GetRandomPosition();
    }

    void Update()
    {
        if (!isInPlayerRange)
            return;

        if (target == null)
            return;

        // �ړ�����
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            targetPosition = GetRandomPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        // ���˃^�C�}�[���J�n
        fireTimer += Time.deltaTime;

        // ���ˊԊu�ɒB������e�𔭎�
        if (fireTimer >= 1f / fireRate)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    private void FireProjectile()
    {
        // �e�̃v���t�@�u����e�I�u�W�F�N�g�𐶐�
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // �e�̕������v�Z
        Vector3 direction = (target.position - firePoint.position).normalized;

        // �e�̑��x��ݒ�
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = direction * projectileSpeed;

        // ��莞�Ԍ�ɒe��j�󂷂�
        Destroy(projectile, projectileLifetime);

        isFiring = true;
    }

    private void StopFiring()
    {
        if (isFiring)
        {
            isFiring = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.gameObject.CompareTag("ebullet"))
        {
            // �^�O��ebullet�̏ꍇ�͏������X�L�b�v
            return;
        }

        if(other.gameObject.CompareTag("Chargebullet"))
        {
            collisionCount += 3;
        }
        else
        {
            collisionCount++;
        }

        // UI�e�L�X�g�̃X�R�A�𑝂₷
        IncreaseScore(1);
        if (collisionCount >= DestroyCount)
        {
            // ��莞�Ԍ�ɔ����𐶐�
            Invoke("Explode", explosionDelay);
        }
        else
        {
            StartCoroutine(FlashMaterial());
        }
    }

    IEnumerator FlashMaterial()
    {
        isFlashing = true;

        // �t���b�V���}�e���A����K�p
        renderer.material = flashMaterial;

        // 0.5�b�ԓG��h�炷
        StartCoroutine(ShakeEnemy(0.5f));

        yield return new WaitForSeconds(flashDuration);

        // ���̃}�e���A���ɖ߂�
        renderer.material = originalMaterial;

        isFlashing = false;
    }

    private void UpdatePlayerRange()
    {
        if (target == null)
            return;

        // �v���C���[�ƓG�̋������v�Z
        float distance = Vector3.Distance(target.position, transform.position);

        // ���������͈͈ȓ��Ȃ�U��
        if (distance <= PlayerDistance)
        {
            isInPlayerRange = true;
        }
        else
        {
            isInPlayerRange = false;
            StopFiring();
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(startPosition.x - movementRangeX, startPosition.x + movementRangeX);
        float randomY = Random.Range(startPosition.y - movementRangeY, startPosition.y + movementRangeY);

        return new Vector3(randomX, randomY, transform.position.z);
    }

    private void LateUpdate()
    {
        UpdatePlayerRange();
    }

    // �X�R�A���X�V
    private void IncreaseScore(int amount)
    {
        int currentScore = int.Parse(scoreText.text);
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    private void Explode()
    {

        audioManager.PlayDestructionSound();

        // �����̃v���t�@�u�𐶐�
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        // �p�[�e�B�N���V�X�e�����擾
        ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();

        // �p�[�e�B�N�����Đ������܂őҋ@
        float waitTime = explosionParticles.main.duration;
        Destroy(explosion, waitTime);

        // �I�u�W�F�N�g��j��
        Destroy(gameObject);
    }

    IEnumerator ShakeEnemy(float duration)
    {
        if (!isShaking)
        {
            isShaking = true;

            Quaternion originalRotation = transform.localRotation;
            float elapsed = 0f;

            audioManager.enemyHitSound();

            while (elapsed < duration)
            {
                float pitch = Random.Range(0f, 0f);
                float yaw = Random.Range(0f, 0f);
                float roll = Random.Range(ShakeDuration1, ShakeDuration2);

                // ���̉�]�ɐV������]��������
                Quaternion newRotation = originalRotation * Quaternion.Euler(pitch, yaw, roll);
                transform.localRotation = newRotation;

                elapsed += Time.unscaledDeltaTime;

                yield return null;
            }

            // ���̉�]�ɖ߂�
            transform.localRotation = originalRotation;
            isShaking = false;
        }
    }

}
