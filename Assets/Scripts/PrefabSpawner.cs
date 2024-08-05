using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject explosionPrefab;  // ��������v���t�@�u
    public float spawnInterval = 1f;    // �����̊Ԋu

    private float timer = 0f;

    void Update()
    {
        // �^�C�}�[�̍X�V
        timer += Time.deltaTime;

        // ���̊Ԋu�Ńv���t�@�u�𐶐�
        if (timer >= spawnInterval)
        {
            SpawnPrefab();
            timer = 0f;  // �^�C�}�[���~
        }
    }

    void SpawnPrefab()
    {
        // �v���t�@�u�𐶐�
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
