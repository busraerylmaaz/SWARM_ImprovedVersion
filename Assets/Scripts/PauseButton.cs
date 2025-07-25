using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenPauseMenu);
    }

    public void OpenPauseMenu()
    {
        Debug.Log("Pause butonuna basýldý.");
        Time.timeScale = 0f;
        SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);
    }
}
