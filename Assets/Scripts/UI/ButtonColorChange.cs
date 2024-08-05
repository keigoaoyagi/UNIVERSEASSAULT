using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    public Color highlightColor;    // �n�C���C�g���̕����F
    private Button button;          // �{�^��
    private Text buttonText;        // �{�^����̃e�L�X�g
    private Color originalColor;    // ���̕����F

    private void Awake()
    {
        buttonText = GetComponentInChildren<Text>();

        if (buttonText == null)
        {
            Debug.LogWarning("Text component not found in the Button's children.");
        }
        else
        {
            originalColor = buttonText.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            // �N���b�N���Ɍ��̐F�ɖ߂�
            buttonText.color = originalColor;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        // �{�^�����I�����ꂽ�Ƃ��̏���
        if (buttonText != null)
        {
            buttonText.color = highlightColor;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }
    public void OnSSelectClick(BaseEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }

    private void OnDisable()
    {
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }

}
