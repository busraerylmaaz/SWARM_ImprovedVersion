// BootSceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSceneLoader : MonoBehaviour
{
    void Start()
    {
        // Managers'lar Awake'te DontDestroyOnLoad ile yüklendiðinde,
        // Main Menu sahnesine geçebiliriz. Küçük bir gecikme eklemek sorunlarý önleyebilir.
        Invoke("LoadMainMenu", 0.1f);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Ana Menü sahnenizin adýný doðru yazdýðýnýzdan emin olun
    }
}