using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfomationUI: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler, IDeselectHandler
{
    // カーソウルがボタン上にあるときのUIやテキストの変数
    public GameObject highlightObject;


    // カーソルがボタンに入った場合
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ボタンの位置にハイライトオブジェクトを移動
        highlightObject.transform.position = transform.position;

        // ハイライトオブジェクトを表示
        highlightObject.SetActive(true);
    }

    // カーソルがボタンから出た場合
    public void OnPointerExit(PointerEventData eventData)
    {
        // ハイライトオブジェクトを非表示
        highlightObject.SetActive(false);
    }

    // ボタンセレクト時の処理
    public void OnSelect(BaseEventData eventData)
    {
        highlightObject.SetActive(true);
    }

    // ボタン非セレクト時の処理
    public void OnDeselect(BaseEventData eventData)
    {
        highlightObject.SetActive(false);
    }

}
