using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Continue butonuna bas�ld�.");
            Time.timeScale = 1f;
            SceneManager.UnloadSceneAsync("Pause Menu");
        });
    }
}
