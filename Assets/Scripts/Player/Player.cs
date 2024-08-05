using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Material flashMaterial;              // フラッシュエフェクトに使用するマテリアル
    public HPBar hpBar;                         // プレイヤーのHPを管理するHPバー
    public GameObject explosionPrefab;          // 爆発エフェクトのプレハブ
    public Image TakeDamageUI;                  // ダメージUIのイメージ
    public GameObject rollEffectPrefab;         // ロールエフェクトのプレハブ
    public Image AIMUI;                         // AIMのUI
    public AudioManager audioManager;           // オーディオマネージャー

    private Quaternion defaultRotation;         // デフォルトの回転
    private Material originalMaterial;          // 元のマテリアル
    private Renderer renderer;                  // レンダラー
    private AudioSource audioSource;            // オーディオソース

    public float movementSpeed = 5f;            // 移動速度
    public float forwardSpeed = 10f;            // 前進速度
    public float rotationSpeed = 20f;           // 回転速度
    public float returnSpeed = 5f;              // 戻り速度
    public float xPositionMinLimit = -5f;       // X軸位置の制限
    public float xPositionMaxLimit = 5f;        
    public float yPositionMinLimit = -5f;       // Y軸位置の制限
    public float yPositionMaxLimit = 5f;
    public float xRotationLimit = 10f;          // X軸の回転制限
    public float yRotationLimit = 10f;          // Y軸の回転制限
    public float zRotationSpeed = 50f;          // Z軸の回転速度
    public float zRotationLimit = 90f;          // Z軸の回転制限
    public float explosionDelay = 3f;           // 爆発の遅延
    public float flashDuration = 1f;            // フラッシュの持続時間
    public float initialDelay = 2.0f;           // 初期遅延
    public int collisionCount = 0;              // 衝突回数
    public int maxCollisionCount = 3;           // 最大衝突回数

    private bool isRotating;                    // 回転中フラグ
    private bool isFlashing = false;            // フラッシング中フラグ
    private bool canInput = false;              // 入力可能フラグ
    private bool isShaking = false;             // 揺れ中フラグ
    private bool isRolling = false;             // ロール中フラグ

    private int rollCount = 0;                  // ロール回数
    private float rollCooldown = 0.5f;          // ロールのクールダウン
    private float lastTapTime = 0f;             // 前回のタップ時間

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();
    }

    void Start()
    {
        // 入力の遅延を設定
        StartCoroutine(DelayInput());
        // デフォルトの回転を保存
        defaultRotation = transform.rotation;
        renderer = GetComponent<Renderer>();
        originalMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // 前方移動
        Vector3 forwardMovement = transform.forward * forwardSpeed * Time.deltaTime;
        transform.position += forwardMovement;

        if (canInput)
        {
            // キー入力を処理
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f) * movementSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + movement;

            newPosition.x = Mathf.Clamp(newPosition.x, xPositionMinLimit, xPositionMaxLimit);
            newPosition.y = Mathf.Clamp(newPosition.y, yPositionMinLimit, yPositionMaxLimit);

            transform.position = newPosition;

            // 回転処理
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                RotateObject(-Vector3.right, xRotationLimit);
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                RotateObject(Vector3.right, xRotationLimit);
            }

            // 縦方向の回転処理
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                RotateObject(-Vector3.right, xRotationLimit);
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                RotateObject(Vector3.right, xRotationLimit);
            }

            // 左右方向の回転処理
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                RotateObject(-Vector3.up, yRotationLimit);
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                RotateObject(Vector3.up, yRotationLimit);
            }

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                RotateObject(-Vector3.up, yRotationLimit);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                RotateObject(Vector3.up, yRotationLimit);
            }

            // Z軸の回転処理
            if (Input.GetKey(KeyCode.E) || Input.GetButton("Rroll"))
            {
                RotateZAxis(-zRotationSpeed, zRotationLimit);
            }
            else if (Input.GetKey(KeyCode.Q) || Input.GetButton("Lroll"))
            {
                RotateZAxis(zRotationSpeed, zRotationLimit);
            }

            if (!isRotating)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, returnSpeed * Time.deltaTime);
            }
            else
            {
                isRotating = false;
            }

            // ダブルタップをチェック
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Rroll") || Input.GetButtonDown("Lroll"))
            {
                StartCoroutine(CheckDoubleTap());
            }

            if (!isRotating && !isRolling)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, returnSpeed * Time.deltaTime);
            }
            else
            {
                isRotating = false;
            }
        }
    }

    // 入力の遅延
    private IEnumerator DelayInput()
    {
        yield return new WaitForSeconds(initialDelay);
        canInput = true;
    }

    // オブジェクトの回転
    private void RotateObject(Vector3 axis, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, axis);
        Quaternion newRotation = transform.rotation * targetRotation;

        transform.rotation = newRotation;

        isRotating = true;
    }

    // Z軸回転
    private void RotateZAxis(float zRotationSpeed, float zRotationLimit)
    {
        float currentZAngle = transform.eulerAngles.z;

        if (currentZAngle > 180f)
        {
            currentZAngle -= 360f;
        }

        float newZAngle = currentZAngle + (zRotationSpeed * Time.deltaTime);

        newZAngle = Mathf.Clamp(newZAngle, -zRotationLimit, zRotationLimit);

        Quaternion currentRotation = transform.rotation;
        Quaternion zRotation = Quaternion.AngleAxis(newZAngle - currentZAngle, Vector3.forward);
        Quaternion newRotation = zRotation * currentRotation;

        transform.rotation = newRotation;

        isRotating = true;
    }

    // トリガーコライダーに侵入したときの処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BossZone"))
        {
            return;
        }
        else
        {
            collisionCount++;
            hpBar.TakeDamage(10f);
            StartCoroutine(DisplayDamageUI());
        }

        if (collisionCount >= maxCollisionCount)
        {
            OnPlayerDeath();
        }
        else
        {
            if (!isFlashing)
            {
                StartCoroutine(FlashMaterial());
            }
        }
    }

    // マテリアルの点滅
    IEnumerator FlashMaterial()
    {
        isFlashing = true;
        StartCoroutine(ShakePlayer(0.5f));
        GetComponent<Renderer>().material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        GetComponent<Renderer>().material = originalMaterial;
        isFlashing = false;
    }

    // 前進を停止
    public void StopForwardMovement()
    {
        forwardSpeed = 0f;
    }

    // プレイヤーの死亡処理
    public void OnPlayerDeath()
    {
        audioManager.PlayDeathSound();
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    // プレイヤーの揺れ
    IEnumerator ShakePlayer(float duration)
    {
        if (!isShaking)
        {
            isShaking = true;
            Quaternion originalRotation = transform.localRotation;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float pitch = Random.Range(0f, 0f);
                float yaw = Random.Range(0f, 0f);
                float roll = Random.Range(-15f, 15f);

                Quaternion pitchRotation = Quaternion.AngleAxis(pitch, Vector3.right);
                Quaternion yawRotation = Quaternion.AngleAxis(yaw, Vector3.up);
                Quaternion rollRotation = Quaternion.AngleAxis(roll, Vector3.forward);

                Quaternion newRotation = originalRotation * pitchRotation * yawRotation * rollRotation;

                transform.localRotation = newRotation;

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            transform.localRotation = originalRotation;
            isShaking = false;
        }
    }

    // ダメージUIの表示
    IEnumerator DisplayDamageUI()
    {
        TakeDamageUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        TakeDamageUI.gameObject.SetActive(false);
    }

    // ダブルタップをチェック
    IEnumerator CheckDoubleTap()
    {
        if (isRolling)
            yield break;

        float doubleTapTime = 0.5f;

        if (Time.time - doubleTapTime < lastTapTime)
        {
            rollCount++;

            if (rollCount == 2)
            {
                StartCoroutine(Roll());
            }
        }
        else
        {
            rollCount = 1;
        }

        lastTapTime = Time.time;
    }

    // ロールの実行
    IEnumerator Roll()
    {
        AIMUI.gameObject.SetActive(false);
        GetComponent<BoxCollider>().isTrigger = false;
        isRolling = true;

        float rollDuration = 1f;
        float elapsed = 0f;

        audioManager.rollSound();

        while (elapsed < rollDuration)
        {
            float rollSpeed = 1800f;
            float rollAngle = rollSpeed * Time.deltaTime;

            transform.Rotate(Vector3.forward, rollAngle);

            Vector3 spawnPosition = transform.position + transform.forward * 4f;
            GameObject rollEffect = Instantiate(rollEffectPrefab, spawnPosition, transform.rotation);

            Destroy(rollEffect, 0.3f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isRolling = false;
        rollCount = 0;

        GetComponent<BoxCollider>().isTrigger = true;
        AIMUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(rollCooldown);
    }
}
