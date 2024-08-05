using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public GameObject uiToShow;                      // �\������UI
    private bool isUIVisible = false;                // �\����ԕێ��̃t���O
    [SerializeField] private Button Selectbutton;    // ���ڐ؂�ւ����ɑI������{�^��

    void Start()
    {
        // ������Ԃł�UI���\��
        uiToShow.SetActive(false);
    }

    void Update()
    {
        // ESC�������ꂽ�ꍇ�̏���
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
        {
            ToggleUIVisibility();

            if (isUIVisible == true)
            {
                Time.timeScale = 0;
                // �w�肵���{�^�����Z���N�g��Ԃɂ���
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
        // �\����Ԃ𔽓]
        isUIVisible = !isUIVisible;

        uiToShow.SetActive(isUIVisible);

    }
}
