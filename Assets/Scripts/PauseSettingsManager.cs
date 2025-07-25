using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettingsManager : MonoBehaviour
{
    [Header("Main Buttons Text")]
    public TMP_Text continueText;
    public TMP_Text settingsText;
    public TMP_Text exitText;

    [Header("Settings Panel Text")]
    public TMP_Text languagesText;
    public TMP_Text musicButtonText;

    [Header("Sliders Panel Text")]
    public TMP_Text audioText;
    public TMP_Text musicText;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider audioSlider;

    [Header("Language Buttons (Optional)")]
    public GameObject turkishButton;
    public GameObject englishButton;

    private LocalizationManager localizationManager;
    private AudioManager audioManager;

    private void Awake()
    {
        SetupManagers();
    }

    private void Start()
    {
        SetupManagers();
        InitializeSliders(); 
    }

    private void OnEnable()
    {
        SetupManagers();
        InitializeSliders(); 
    }

    private void OnDisable()
    {
        if (localizationManager != null)
            localizationManager.languageChanged -= UpdateTexts;
    }

    private void SetupManagers()
    {
        if (LocalizationManager.Instance != null)
        {
            localizationManager = LocalizationManager.Instance;
            localizationManager.languageChanged -= UpdateTexts;
            localizationManager.languageChanged += UpdateTexts;
            UpdateTexts(true);
        }

        if (AudioManager.Instance != null)
        {
            audioManager = AudioManager.Instance;
        }
    }

    private void InitializeSliders()
    {
        if (audioManager == null) return;

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.value = audioManager.GetMusicVolume();
            SetMusicVolume(musicSlider.value); 
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (audioSlider != null)
        {
            audioSlider.onValueChanged.RemoveAllListeners();
            audioSlider.value = audioManager.GetSfxVolume();
            SetSfxVolume(audioSlider.value); 
            audioSlider.onValueChanged.AddListener(SetSfxVolume);
        }
    }

    private void UpdateTexts(bool _)
    {
        if (localizationManager == null) return;

        if (continueText != null)
            continueText.text = localizationManager.GetLocalizedValue("menu_continue");

        if (settingsText != null)
            settingsText.text = localizationManager.GetLocalizedValue("menu_settings");

        if (exitText != null)
            exitText.text = localizationManager.GetLocalizedValue("menu_exit");

        if (languagesText != null)
            languagesText.text = localizationManager.GetLocalizedValue("menu_languages");

        if (musicButtonText != null)
            musicButtonText.text = localizationManager.GetLocalizedValue("menu_music");

        if (musicText != null)
            musicText.text = localizationManager.GetLocalizedValue("menu_music");

        if (audioText != null)
            audioText.text = localizationManager.GetLocalizedValue("menu_audio");
    }

    public void SetLanguageToEnglish() => localizationManager?.SetLanguage("en");
    public void SetLanguageToTurkish() => localizationManager?.SetLanguage("tr");

    public void SetMusicVolume(float volume)
    {
        Debug.Log("[PauseSettingsManager] SetMusicVolume: " + volume);
        audioManager?.SetMusicVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        Debug.Log("[PauseSettingsManager] SetSfxVolume: " + volume);
        audioManager?.SetSfxVolume(volume);
    }
}
