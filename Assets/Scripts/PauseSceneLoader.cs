using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseSceneLoader : MonoBehaviour
{
    public Button pauseButton;

    void Start()
    {
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OpenPauseMenu);
        }
        else
        {
            Debug.LogError("Pause button atanmadý! Inspector'da sürükleyip býrakmayý unutma.");
        }
    }

    public void OpenPauseMenu()
    {
        Debug.Log("Pause butonuna basýldý — sahne yükleniyor...");

        
        Time.timeScale = 0f;

       
        SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
    }
}
