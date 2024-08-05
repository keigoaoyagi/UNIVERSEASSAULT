using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    private Vector3 initialPosition; // 初期位置

    public float shakeAmount = 0.1f; // 揺れの強さ
    public float shakeSpeed = 2f;    // 揺れの速さ


    void Start()
    {
        initialPosition = transform.position; // 初期位置を保存
    }

    void Update()
    {
        // y方向の振動
        float yOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        transform.position = initialPosition + new Vector3(0f, yOffset, 0f);
    }
}
