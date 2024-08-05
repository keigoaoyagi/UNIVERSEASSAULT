using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public GameObject uiToShow;                      // 表示するUI
    private bool isUIVisible = false;                // 表示状態保持のフラグ
    [SerializeField] private Button Selectbutton;    // 項目切り替え時に選択するボタン

    void Start()
    {
        // 初期状態ではUIを非表示
        uiToShow.SetActive(false);
    }

    void Update()
    {
        // ESCが押された場合の処理
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
        {
            ToggleUIVisibility();

            if (isUIVisible == true)
            {
                Time.timeScale = 0;
                // 指定したボタンをセレクト状態にする
                Selectbutton.Select();
            }

        }

        if (isUIVisible == false)
        {
            Time.timeScale = 1;
        }

    }

    private void ToggleUIVisibility()
    {
        // 表示状態を反転
        isUIVisible = !isUIVisible;

        uiToShow.SetActive(isUIVisible);

    }
}
