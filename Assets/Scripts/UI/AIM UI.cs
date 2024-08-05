using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIMUI : MonoBehaviour
{
    public Transform target3DObject; // 追従する3Dオブジェクト
    public Image imageToMove; // 動かすUI
    public Camera mainCamera; // メインのカメラ

    private RectTransform imageRectTransform;

    void Start()
    {
        // UIイメージの位置を取得
        imageRectTransform = imageToMove.GetComponent<RectTransform>();
    }

    void Update()
    {
        // 3Dオブジェクトの位置をスクリーン座標に変換
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target3DObject.position);

        // スクリーン座標をUIのRectTransformに適用
        imageRectTransform.position = screenPosition;
    }
}
