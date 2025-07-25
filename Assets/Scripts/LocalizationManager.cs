using System;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    
    private static LocalizationManager _instance; 

    public static LocalizationManager Instance 
    {
        get
        {
            
            if (_instance == null)
            {
                
                _instance = FindObjectOfType<LocalizationManager>();

                
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<LocalizationManager>();
                    singletonObject.name = typeof(LocalizationManager).ToString() + " (Singleton)";
                }

                
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    
    private ILocalization currentLocalization;
    public event Action<bool> languageChanged;
    public string currentLanguage;

    private void Awake()
    {
        
        
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        
        else if (_instance == null)
        {
            _instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
       
        
        LoadLanguageFromPrefs();
    }

    private void LoadLanguageFromPrefs()
    {
        currentLanguage = PlayerPrefs.GetString("lang", "en");
        ApplyLanguage(currentLanguage);
    }

    public void SetLanguage(string languageCode)
    {
        if (string.IsNullOrEmpty(languageCode)) return;

        PlayerPrefs.SetString("lang", languageCode);
        PlayerPrefs.Save();

        ApplyLanguage(languageCode);
    }

    private void ApplyLanguage(string code)
    {
        switch (code)
        {
            case "en":
                currentLocalization = new LocalizationEN();
                currentLanguage = "en";
                break;
            case "tr":
                currentLocalization = new LocalizationTR();
                currentLanguage = "tr";
                break;
            default:
                currentLocalization = new LocalizationEN();
                currentLanguage = "en";
                break;
        }

        languageChanged?.Invoke(true);
    }

    public string GetLocalizedValue(string key)
    {
        if (currentLocalization == null) return key;
        return currentLocalization.GetValue(key) ?? key;
    }
}