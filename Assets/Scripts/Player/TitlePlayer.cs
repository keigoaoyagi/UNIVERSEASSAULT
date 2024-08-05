using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    private Vector3 initialPosition; // �����ʒu

    public float shakeAmount = 0.1f; // �h��̋���
    public float shakeSpeed = 2f;    // �h��̑���


    void Start()
    {
        initialPosition = transform.position; // �����ʒu��ۑ�
    }

    void Update()
    {
        // y�����̐U��
        float yOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        transform.position = initialPosition + new Vector3(0f, yOffset, 0f);
    }
}
