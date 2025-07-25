using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMenuManager : MonoBehaviour
{
    private LocalizationManager localizationManager;
    public TMP_Text noxText;
    public TMP_Text aresText;
    public TMP_Text characterText;
    public TMP_Text backButtonText;

    void Awake()
    {
        SetupManagers();
    }

    void OnEnable()
    {
        SetupManagers();
    }

    void OnDisable()
    {
        if (localizationManager != null)
        {
            localizationManager.languageChanged -= SetText;
        }
    }

    private void SetupManagers()
    {
        localizationManager = LocalizationManager.Instance;

        if (localizationManager != null)
        {
            localizationManager.languageChanged -= SetText; 
            localizationManager.languageChanged += SetText; 
            SetText(true); 
            Debug.Log("CharacterMenuManager: LocalizationManager Instance kurulumu baþarýlý.");
        }
        else
        {
            Debug.LogError("CharacterMenuManager: LocalizationManager Instance bulunamadý!");
        }
    }

    private void SetText(bool _)
    {
        if (localizationManager == null) return;

        noxText.text = localizationManager.GetLocalizedValue("nox_summary");
        aresText.text = localizationManager.GetLocalizedValue("ares_summary");
        characterText.text = localizationManager.GetLocalizedValue("character");
        backButtonText.text = localizationManager.GetLocalizedValue("back");
        Debug.Log("Character Menu UI metinleri güncellendi.");
    }

    public void SelectCharacter(string characterName)
    {
        PlayerLevelManager.Instance.UpdateCharacter(characterName);
        SceneManager.LoadScene("level1");
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); 
    }
}