using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    // 非表示にする項目を参照
    [SerializeField] private GameObject[] hideItems;
    // 表示する次の項目を参照
    [SerializeField] private GameObject nextItem;
    // 項目切替時に選択するボタン
    [SerializeField] private Button Selectbutton;

    // 現在の表示インデックス
    private int currentIndex = 0;

    // ボタンクリック時の処理
    public void OnButtonClick()
    {
        // 現在表示中の項目を非表示
        if (currentIndex >= 0 && currentIndex < hideItems.Length)
        {
            hideItems[currentIndex].SetActive(false);
        }

        // 次の項目をアクティブ
        if (nextItem != null)
        {
            nextItem.SetActive(true);
            Selectbutton.Select();

        }

        // インデックスを更新
        currentIndex++;

        // インデックスが範囲外になったらリセットする
        if (currentIndex >= hideItems.Length)
        {
            currentIndex = 0;
        }
    }

}
