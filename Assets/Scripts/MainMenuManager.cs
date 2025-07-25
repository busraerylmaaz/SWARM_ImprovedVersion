using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text startText;
    public TMP_Text settingsText;
    public TMP_Text quitText;
    public TMP_Text languagesText;
    public TMP_Text musicButtonText;
    public TMP_Text audioText;
    public TMP_Text musicText;

    public Slider MusicSlider;
    public Slider AudioSlider;

    private LocalizationManager localizationManager;
    private AudioManager audioManager;

    private void Awake()
    {
        SetupManagers();
    }

    private void Start()
    {
        SetupManagers();

        if (audioManager != null)
        {
            if (MusicSlider != null)
            {
                MusicSlider.onValueChanged.RemoveAllListeners();
                MusicSlider.value = audioManager.GetMusicVolume();
                SetMusicVolume(MusicSlider.value); 
                MusicSlider.onValueChanged.AddListener(SetMusicVolume);
            }

            if (AudioSlider != null)
            {
                AudioSlider.onValueChanged.RemoveAllListeners();
                AudioSlider.value = audioManager.GetSfxVolume();
                SetSfxVolume(AudioSlider.value); 
                AudioSlider.onValueChanged.AddListener(SetSfxVolume);
            }
        }
    }

    private void OnEnable()
    {
        SetupManagers();
    }

    private void OnDisable()
    {
        if (localizationManager != null)
            localizationManager.languageChanged -= SetText;
    }

    private void SetupManagers()
    {
        if (LocalizationManager.Instance != null)
        {
            localizationManager = LocalizationManager.Instance;
            localizationManager.languageChanged -= SetText;
            localizationManager.languageChanged += SetText;
            SetText(true);
        }

        if (AudioManager.Instance != null)
        {
            audioManager = AudioManager.Instance;
        }
    }

    private void SetText(bool _)
    {
        if (localizationManager == null) return;

        startText.text = localizationManager.GetLocalizedValue("menu_start");
        settingsText.text = localizationManager.GetLocalizedValue("menu_settings");
        quitText.text = localizationManager.GetLocalizedValue("menu_exit");
        languagesText.text = localizationManager.GetLocalizedValue("menu_languages");
        musicButtonText.text = localizationManager.GetLocalizedValue("menu_music");
        musicText.text = localizationManager.GetLocalizedValue("menu_music");
        audioText.text = localizationManager.GetLocalizedValue("menu_audio");
    }

    public void SetLanguageToEnglish() => localizationManager?.SetLanguage("en");
    public void SetLanguageToTurkish() => localizationManager?.SetLanguage("tr");

    public void SetMusicVolume(float volume) => audioManager?.SetMusicVolume(volume);
    public void SetSfxVolume(float volume) => audioManager?.SetSfxVolume(volume);

    public void StartGame() => SceneManager.LoadScene("Character Selection");
    public void QuitGame() => Application.Quit();
}
