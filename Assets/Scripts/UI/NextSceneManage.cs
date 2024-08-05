using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneManage : MonoBehaviour
{
    public string NextSceneName;        
    public Image fadeImage;             
    public float fadeDuration = 1.0f;   

    private bool isFading = false;
    private float fadeTimer = 0f;

    void Update()
    {
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha = fadeTimer / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);

            if (fadeTimer >= fadeDuration)
            {
                SceneManager.LoadScene(NextSceneName); 
            }
        }
    }

    public void OnButtonClick()
    {
        if (!isFading)
        {
            Time.timeScale = 1.0f;
            isFading = true;       
        }
    }
}
