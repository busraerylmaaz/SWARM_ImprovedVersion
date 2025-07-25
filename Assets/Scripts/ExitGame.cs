
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitGame : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ExitApp);
    }

    void ExitApp()
    {
        Debug.Log("��k�� yap�l�yor...");

        
        Application.Quit();

        
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
