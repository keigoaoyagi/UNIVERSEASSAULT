using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Boss : MonoBehaviour
{
    public Transform target;                // �ǔ��Ώۂ̃g�����X�t�H�[��
    public Transform firePoint1;            // �e�𔭎˂���ʒu
    public Transform firePoint2;            
    public GameObject bulletPrefab;         // �e�̃v���t�@�u
    public ParticleSystem particleSystem;   // �e���Đ�����p�[�e�B�N���V�X�e��
    public Text scoreText;                  // �X�R�A��\������UI�e�L�X�g
    public BossHP hpBar;                    // �{�X��HP�o�[�Q��
    public Material flashMaterial;          // �_�Ŏ��̃}�e���A��
    private Material originalMaterial;      // ���̃}�e���A��
    private Quaternion initialRotation;     // �����̉�]�l
    private Renderer renderer;              // �����_���[�R���|�[�l���g

    public float moveSpeed = 3f;            // �{�X�̈ړ����x
    public float attackInterval = 2f;       // �U���̊Ԋu
    public float bulletSpeed = 10f;         // �e�̑��x
    public float particleDuration = 3f;     // �p�[�e�B�N���̍Đ�����
    public float playDuration = 0f;
    public int maxCollisionCount = 5;       // �ő�Փˉ�


    private float attackTimer = 0f;         // �U���̃^�C�}�[
    private float playTime = 0f;
    private bool isFlashing = false;        // �_�Ŏ��̃t���O
    private bool isFireing = false;         // �U���̃t���O
    private bool isRotationLocked = false;  // ��]�Œ�t���O
    private bool isPlaying = false;         // �p�[�e�B�N���Đ����̃t���O
    private int collisionCount = 0;         // �ő�Փˉ񐔂̃J�E���g

    public string NextSceneName;            // �ڍs��̃V�[���̎w��

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material; // ���̃}�e���A����ۑ�
    }

    void Update()
    {
        if (isRotationLocked == false)
        {
            Tracking();
        }

        if (isFireing == true)
        {
            if (target == null)
                return;

            // �e�𔭎˂���^�C�~���O���Ǘ�
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                // �����_���Ȓl�𐶐�
                float randomValue = Random.Range(0, 3);

                if (randomValue == 0)
                {
                    // �e�𔭎�
                    Attack();
                }
                else if (randomValue == 1)
                {
                    isRotationLocked = true;
                    initialRotation = transform.rotation; // �����̉�]�l��ۑ�
                    transform.rotation = initialRotation; // �I�u�W�F�N�g�̉�]�l�������̉�]�n�ɌŒ�

                    if (isRotationLocked)
                    {
                        // �p�[�e�B�N�����Đ�
                        Invoke("PlayParticles",2);
                    }
                }
                else if(randomValue == 2)
                {

                }

                attackTimer = 0f; // �^�C�}�[�����Z�b�g
            }

            if (isPlaying)
            {
                // �o�ߎ��Ԃ��v�Z
                playTime += Time.deltaTime;

                // �Đ����ԂɒB������Đ����~
                if (playTime >= playDuration)
                {
                    Stop();
                }
            }
        }
    }

    private void Attack()
    {


        // �e�̃v���t�@�u����e�I�u�W�F�N�g�𐶐�
        GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);

        // �e�̑��x��ݒ�
        Rigidbody bulletRigidbody1 = bullet1.GetComponent<Rigidbody>();
        Vector3 bulletDirection1 = (target.position - firePoint1.position).normalized;
        bulletRigidbody1.velocity = bulletDirection1 * bulletSpeed; 

        Rigidbody bulletRigidbody2 = bullet2.GetComponent<Rigidbody>();
        Vector3 bulletDirection2 = (target.position - firePoint2.position).normalized;
        bulletRigidbody2.velocity = bulletDirection2 * bulletSpeed;

        // ��莞�Ԍ�ɒe��j�󂷂�
        Destroy(bullet1, 5f);
        Destroy(bullet2, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chargebullet"))
        {
            collisionCount += 10;
            // HP�o�[������������
            hpBar.TakeDamage(10f);
        }
        else
        {
            collisionCount++;
            // HP�o�[������������
            hpBar.TakeDamage(1f);
        }


        // UI�e�L�X�g�̃X�R�A�𑝂₷
        IncreaseScore(1);

        // �w��񐔂̏Փ˂��������玩�g��j��
        if (collisionCount >= maxCollisionCount)
        {

            LoadNextScene();

            Destroy(gameObject);
        }
        else
        {
            if (!isFlashing)
            {
                StartCoroutine(FlashMaterial());
            }
        }
    }

    private IEnumerator FlashMaterial()
    {
        isFlashing = true;

        // �t���b�V���}�e���A����K�p
        renderer.material = flashMaterial;

        // �t���b�V���̎���
        yield return new WaitForSeconds(0.2f);

        // ���̃}�e���A���ɖ߂�
        renderer.material = originalMaterial;

        isFlashing = false;
    }

    private void IncreaseScore(int amount)
    {
        int currentScore = int.Parse(scoreText.text);
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    public void StartBossProcess()
    {
        isFireing = true;
    }

    private void PlayParticles()
    {
        isPlaying = true;

        // �p�[�e�B�N�����Đ�
        particleSystem.Play();
    }

    private void Stop()
    {
        particleSystem.Stop();
        isRotationLocked = false;
        isPlaying = false;
        playTime = 0f;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }

    private void Tracking()
    {
        if (isPlaying != true)
        {
            //��]�Œ肪�������ꂽ�ꍇ�A�I�u�W�F�N�g�̉�]���X�V
            Vector3 targetDirection = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }

    }
}