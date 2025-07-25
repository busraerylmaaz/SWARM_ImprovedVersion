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
            Debug.LogError("Pause button atanmad�! Inspector'da s�r�kleyip b�rakmay� unutma.");
        }
    }

    public void OpenPauseMenu()
    {
        Debug.Log("Pause butonuna bas�ld� � sahne y�kleniyor...");

        
        Time.timeScale = 0f;

       
        SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
    }
}
