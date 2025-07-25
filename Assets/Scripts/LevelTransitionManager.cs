using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelTransitionManager : MonoBehaviour
{
    public TMP_Text playerText;
    public TMP_Text gunText;
    public TMP_Text modifiersText;
    public TMP_Text mobsKilledText;
    public TMP_Text timeRemainingText;
    //public TMP_Text exitText;

    public TMP_Text levelSummaryLabelText;
    public TMP_Text resultLabelText;
    public TMP_Text characterLabelText;
    public TMP_Text gunLabelText;
    public TMP_Text mobCountLabelText;
    public TMP_Text timeRemainingLabelText;
    public TMP_Text nextStageLabelText;
    public List<TMP_Text> modifierPlaceholders;
    public GameManager gameManager;
    public LocalizationManager localizationManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        localizationManager = LocalizationManager.Instance;

        resultLabelText.text = gameManager.levelWon == 1 ? "Victory!" : "Defeat!";
        if (gameManager.levelWon == 1)
        {
            resultLabelText.color = Color.green;
        }
        else
        {
            resultLabelText.color = Color.red;
        }

        characterLabelText.text = gameManager.characterName;
        gunLabelText.text = localizationManager.GetLocalizedValue(gameManager.gunName);
        mobCountLabelText.text = gameManager.mobCount.ToString();
        timeRemainingLabelText.text = Mathf.RoundToInt(gameManager.timeRemaining).ToString();
        nextStageLabelText.text = gameManager.levelWon == 1 ? "Market" : "Retry";
        Translate();
        DisplayActiveModifiers();
    }

    void Translate()
    {
        playerText.text = localizationManager.GetLocalizedValue("player");
        gunText.text = localizationManager.GetLocalizedValue("gun");
        modifiersText.text = localizationManager.GetLocalizedValue("modifiers");
        mobsKilledText.text = localizationManager.GetLocalizedValue("mobs_killed");
        timeRemainingText.text = localizationManager.GetLocalizedValue("time_remaining");
        //exitText.text = localizationManager.GetLocalizedValue("menu_exit");

        levelSummaryLabelText.text = localizationManager.GetLocalizedValue("level_summary") + ":" + gameManager.level.ToString();
        resultLabelText.text = gameManager.levelWon == 1 ? localizationManager.GetLocalizedValue("victory") : localizationManager.GetLocalizedValue("defeat");
        nextStageLabelText.text = gameManager.levelWon == 1 ? localizationManager.GetLocalizedValue("market") : localizationManager.GetLocalizedValue("retry");
    }

    private void DisplayActiveModifiers()
    {
        var activeModifiers = new Dictionary<string, (int count, string color, bool isPositive)>();

        void FormatModifier(float value, string label, out string key, out string color, out bool isPositive)
        {
            if (value > 1f)
            {
                int percent = Mathf.RoundToInt((value - 1f) * 100);
                key = $"+{percent}% {label.ToUpper()}";
                color = "#55BB5A";
                isPositive = true;
            }
            else if (value < 1f)
            {
                int percent = Mathf.Abs(Mathf.RoundToInt((value - 1f) * 100));
                key = $"-{percent}% {label.ToUpper()}";
                color = "#D72048";
                isPositive = false;
            }
            else
            {
                key = null;
                color = null;
                isPositive = true;
            }
        }

        void AddModifier(float value, string label)
        {
            FormatModifier(value, label, out string key, out string color, out bool isPositive);
            if (key != null)
            {
                if (activeModifiers.ContainsKey(key))
                    activeModifiers[key] = (activeModifiers[key].count + 1, color, isPositive);
                else
                    activeModifiers[key] = (1, color, isPositive);
            }
        }

        AddModifier(gameManager.modifiers.maxHealthModifier, "Max Health");
        AddModifier(gameManager.modifiers.speedModifier, "Speed");
        AddModifier(gameManager.modifiers.damageModifier, "Damage");
        AddModifier(gameManager.modifiers.fireRateModifier, "Fire Rate");
        AddModifier(gameManager.modifiers.dodgeChanceModifier, "Dodge Chance");
        AddModifier(gameManager.modifiers.maxAmmoModifier, "Max Ammo");

      
        var sortedModifiers = new List<KeyValuePair<string, (int count, string color, bool isPositive)>>(activeModifiers);
        sortedModifiers.Sort((a, b) =>
        {
            int comparison = b.Value.isPositive.CompareTo(a.Value.isPositive);
            if (comparison == 0)
                return a.Key.CompareTo(b.Key); 
            return comparison;
        });

        int index = 0;
        foreach (var pair in sortedModifiers)
        {
            if (index >= modifierPlaceholders.Count) break;

            string key = pair.Key;
            int count = pair.Value.count;
            string color = pair.Value.color;

            string countText = count > 1 ? $" x{count}" : ""; 
            modifierPlaceholders[index].text = $"<color={color}>{key}</color>{countText}";
            modifierPlaceholders[index].gameObject.SetActive(true);
            index++;
        }

        for (; index < modifierPlaceholders.Count; index++)
        {
            modifierPlaceholders[index].gameObject.SetActive(false);
        }
    }




    public void ExitGame()
    {
        Debug.Log("LevelTransitionManager'dan ExitGame() ?a?r?ld?. Oyun modu durduruluyor.");

        Application.Quit();
    }

    public void Advance()
    {
        if (gameManager.levelWon == 1)
        {
            SceneManager.LoadScene("Market Menu");
        }
        else
        {

            Destroy(GameManager.Instance.gameObject);
            Destroy(PlayerLevelManager.Instance.gameObject);
            Destroy(LocalizationManager.Instance.gameObject);
            Destroy(MarketConfig.Instance.gameObject);
            SceneManager.LoadScene("Main Menu");
        }
    }


}
