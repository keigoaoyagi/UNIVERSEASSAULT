using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIMUI : MonoBehaviour
{
    public Transform target3DObject; // �Ǐ]����3D�I�u�W�F�N�g
    public Image imageToMove; // ������UI
    public Camera mainCamera; // ���C���̃J����

    private RectTransform imageRectTransform;

    void Start()
    {
        // UI�C���[�W�̈ʒu���擾
        imageRectTransform = imageToMove.GetComponent<RectTransform>();
    }

    void Update()
    {
        // 3D�I�u�W�F�N�g�̈ʒu���X�N���[�����W�ɕϊ�
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target3DObject.position);

        // �X�N���[�����W��UI��RectTransform�ɓK�p
        imageRectTransform.position = screenPosition;
    }
}
