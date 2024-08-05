using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDestroy : MonoBehaviour
{
    public Material blinkMaterial;          // �_�Ŏ��Ɏg�p����}�e���A��
    public GameObject explosionPrefab;      // �����̃v���t�@�u

    private Renderer renderer;              // �I�u�W�F�N�g�̃����_���[
    private Material originalMaterial;      // ���̃}�e���A��

    public float blinkDuration = 2f;        // �_�ł̊��ԁi�b�j
    public float explosionDelay = 3f;       // �����̒x�����ԁi�b�j

    private bool isBlinking = false;        // �_�Œ��̃t���O
    private float blinkTimer = 0f;          // �_�Ń^�C�}�[


    private void Start()
    {

        // �I�u�W�F�N�g�̃����_���[���擾
        renderer = GetComponent<Renderer>();

        // �I�u�W�F�N�g�̌��̃}�e���A����ۑ�
        originalMaterial = renderer.material;

        // �I�u�W�F�N�g��_�ł�����
        StartBlinking();

        // ��莞�Ԍ�ɔ���
        Invoke("Explode", explosionDelay);

    }

    private void Update()
    {
        if (isBlinking)
        {
            // �_�ł̃^�C�~���O���v�Z
            blinkTimer += Time.deltaTime;

            // �_�ł̎����ɍ��킹�ă}�e���A����؂�ւ���
            if (blinkTimer >= blinkDuration)
            {
                if (renderer.material == originalMaterial)
                {
                    renderer.material = blinkMaterial;
                }
                else
                {
                    renderer.material = originalMaterial;
                }

                blinkTimer = 0f; // �^�C�}�[�����Z�b�g
            }
        }

    }

    private void StartBlinking()
    {
        isBlinking = true;
        blinkTimer = 0f; // �^�C�}�[�����Z�b�g
    }

    public void StopBlinking()
    {
        isBlinking = false;
        renderer.material = originalMaterial; // �}�e���A�������ɖ߂�
    }

    private void Explode()
    {
       
        
        // �����̃v���t�@�u�𐶐�
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        // �I�u�W�F�N�g��j��
        Destroy(gameObject);
    }
}

