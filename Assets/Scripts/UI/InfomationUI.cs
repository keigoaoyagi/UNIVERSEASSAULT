using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfomationUI: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler, IDeselectHandler
{
    // �J�[�\�E�����{�^����ɂ���Ƃ���UI��e�L�X�g�̕ϐ�
    public GameObject highlightObject;


    // �J�[�\�����{�^���ɓ������ꍇ
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �{�^���̈ʒu�Ƀn�C���C�g�I�u�W�F�N�g���ړ�
        highlightObject.transform.position = transform.position;

        // �n�C���C�g�I�u�W�F�N�g��\��
        highlightObject.SetActive(true);
    }

    // �J�[�\�����{�^������o���ꍇ
    public void OnPointerExit(PointerEventData eventData)
    {
        // �n�C���C�g�I�u�W�F�N�g���\��
        highlightObject.SetActive(false);
    }

    // �{�^���Z���N�g���̏���
    public void OnSelect(BaseEventData eventData)
    {
        highlightObject.SetActive(true);
    }

    // �{�^����Z���N�g���̏���
    public void OnDeselect(BaseEventData eventData)
    {
        highlightObject.SetActive(false);
    }

}
