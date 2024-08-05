using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour
{
    private Vector3 initialPosition; // ‰ŠúˆÊ’u

    public float shakeAmount = 0.1f; // —h‚ê‚Ì‹­‚³
    public float shakeSpeed = 2f;    // —h‚ê‚Ì‘¬‚³


    void Start()
    {
        initialPosition = transform.position; // ‰ŠúˆÊ’u‚ğ•Û‘¶
    }

    void Update()
    {
        // y•ûŒü‚ÌU“®
        float yOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
        transform.position = initialPosition + new Vector3(0f, yOffset, 0f);
    }
}
