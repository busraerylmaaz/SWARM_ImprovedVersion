// BootSceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSceneLoader : MonoBehaviour
{
    void Start()
    {
        // Managers'lar Awake'te DontDestroyOnLoad ile y�klendi�inde,
        // Main Menu sahnesine ge�ebiliriz. K���k bir gecikme eklemek sorunlar� �nleyebilir.
        Invoke("LoadMainMenu", 0.1f);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Ana Men� sahnenizin ad�n� do�ru yazd���n�zdan emin olun
    }
}