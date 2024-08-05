using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    // ��\���ɂ��鍀�ڂ��Q��
    [SerializeField] private GameObject[] hideItems;
    // �\�����鎟�̍��ڂ��Q��
    [SerializeField] private GameObject nextItem;
    // ���ڐؑ֎��ɑI������{�^��
    [SerializeField] private Button Selectbutton;

    // ���݂̕\���C���f�b�N�X
    private int currentIndex = 0;

    // �{�^���N���b�N���̏���
    public void OnButtonClick()
    {
        // ���ݕ\�����̍��ڂ��\��
        if (currentIndex >= 0 && currentIndex < hideItems.Length)
        {
            hideItems[currentIndex].SetActive(false);
        }

        // ���̍��ڂ��A�N�e�B�u
        if (nextItem != null)
        {
            nextItem.SetActive(true);
            Selectbutton.Select();

        }

        // �C���f�b�N�X���X�V
        currentIndex++;

        // �C���f�b�N�X���͈͊O�ɂȂ����烊�Z�b�g����
        if (currentIndex >= hideItems.Length)
        {
            currentIndex = 0;
        }
    }

}
