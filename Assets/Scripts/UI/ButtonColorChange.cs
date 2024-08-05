using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    public Color highlightColor;    // ハイライト時の文字色
    private Button button;          // ボタン
    private Text buttonText;        // ボタン上のテキスト
    private Color originalColor;    // 元の文字色

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
            // クリック時に元の色に戻す
            buttonText.color = originalColor;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        // ボタンが選択されたときの処理
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
