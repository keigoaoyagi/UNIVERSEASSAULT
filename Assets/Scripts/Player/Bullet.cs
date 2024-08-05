using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject firingPoint;

    [SerializeField]
    private GameObject normalBullet;

    [SerializeField]
    private GameObject chargedBullet;

    [SerializeField]
    private Text chargeText; //　チャージ中のテキスト

    public float speed = 30f;

    private AudioSource audioSource;
    public AudioManager audioManager;

    private float minChargeTime = 0.5f;
    private float maxChargeTime = 1.5f;
    private float currentChargeTime = 0f;
    private bool isCharging = false;

    private bool isTextActive = false; // テキスト表示中のフラグ

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1"))
        {
            FireNormalShot();
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Fire2"))
        {
            if (!isCharging)
            {
                audioManager.ChargeingSound();
                StartCharging();
            }

            currentChargeTime += Time.deltaTime;
            currentChargeTime = Mathf.Clamp(currentChargeTime, minChargeTime, maxChargeTime);

            if (!isTextActive && currentChargeTime > 0)
            {
                chargeText.gameObject.SetActive(true);
                isTextActive = true;
            }
        }

        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2")) && isCharging)
        {
            if (currentChargeTime == maxChargeTime)
            {
                FireChargedBullet();
            }
            ResetCharge();
            StopBlinking();
        }
    }

    private void StartCharging()
    {
        isCharging = true;

        // チャージ時にアクティブ
        if (currentChargeTime > 0 && !isTextActive)
        {
            chargeText.gameObject.SetActive(true);
            isTextActive = true;
        }

        StartCoroutine(BlinkText());
    }

    private void StopBlinking()
    {
        StopCoroutine(BlinkText());
        chargeText.gameObject.SetActive(false);
        isTextActive = false;
    }

    private IEnumerator BlinkText()
    {
        while (isCharging)
        {
            chargeText.CrossFadeAlpha(0.0f, 0.5f, false);
            yield return new WaitForSeconds(0.5f);
            chargeText.CrossFadeAlpha(1.0f, 0.5f, false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FireNormalShot()
    {
        Vector3 bulletPosition = firingPoint.transform.position;
        GameObject newBullet = Instantiate(normalBullet, bulletPosition, transform.rotation);
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(newBullet, 0.5f);
        audioSource.Play();
    }

    private void FireChargedBullet()
    {
        Vector3 bulletPosition = firingPoint.transform.position;
        GameObject newBullet = Instantiate(chargedBullet, bulletPosition, transform.rotation);
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        float speedMultiplier = currentChargeTime / maxChargeTime;
        bulletRigidbody.AddForce(transform.forward * speed * speedMultiplier, ForceMode.Impulse);
        Destroy(newBullet, 0.5f);
        audioManager.ChargeShot();
    }

    private void ResetCharge()
    {
        isCharging = false;
        currentChargeTime = 0f;
    }


}
