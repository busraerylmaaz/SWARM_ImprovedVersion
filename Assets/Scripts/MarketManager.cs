using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MarketManager : MonoBehaviour
{
    private MarketConfig marketConfig;
    private PlayerLevelManager playerLevelManager;
    private GameManager gameManager;
    private LocalizationManager localizationManager; 

    private string itemKey;
    public TMP_Text coinText;
    public TMP_Text continueText;

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
            localizationManager.languageChanged -= UpdateUIOnLanguageChange;
        }
    }

    private void SetupManagers()
    {
        if (LocalizationManager.Instance != null)
        {
            localizationManager = LocalizationManager.Instance;
            
            localizationManager.languageChanged -= UpdateUIOnLanguageChange;
            localizationManager.languageChanged += UpdateUIOnLanguageChange;
            UpdateUIOnLanguageChange(true); 
            Debug.Log("MarketManager: LocalizationManager kurulumu ba?ar?l?.");
        }
        else
        {
            Debug.LogError("MarketManager: LocalizationManager Instance bulunamad?!");
        }

        marketConfig = MarketConfig.Instance;
        playerLevelManager = PlayerLevelManager.Instance;
        gameManager = GameManager.Instance;

      
        if (playerLevelManager != null)
        {
            coinText.text = playerLevelManager.GetCurrency().ToString();
        }
        else
        {
            Debug.LogError("MarketManager: PlayerLevelManager Instance bulunamad?!");
        }
    }

    private void UpdateUIOnLanguageChange(bool _) 
    {
        if (localizationManager == null) return;
        continueText.text = localizationManager.GetLocalizedValue("continue");
        
    }

    public void SetItemKey(string key)
    {
        itemKey = key;
    }

    public void BuyItem()
    {
      
        if (marketConfig == null || !marketConfig.Items.TryGetValue(itemKey, out var itemData))
        {
            Debug.LogError($"Item key {itemKey} not found in market config or marketConfig is null.");
            return;
        }
        int price = itemData.price;
       
        int currency = playerLevelManager != null ? playerLevelManager.GetCurrency() : 0;

        if (currency < price)
        {
            return;
        }
        if (itemData.type == "skill")
        {
            string ability = itemData.ability;
            if (float.TryParse(itemData.ability_effect.ToString(), out float effectValue))
            {
                ApplySkill(ability, effectValue);
            }
        }
        else if (itemData.type == "weapon")
        {
            ApplyWeapon(itemData.name);
        }

       
        if (playerLevelManager != null)
        {
            playerLevelManager.SubTrackCurrency(itemData.price);
            coinText.text = playerLevelManager.GetCurrency().ToString();
        }
    }

    private void ApplyWeapon(string weaponName)
    {
      
        if (playerLevelManager == null) { Debug.LogError("PlayerLevelManager is null in ApplyWeapon."); return; }
        switch (weaponName)
        {
            case "assault_rifle_name":
                playerLevelManager.UpdateWeapon("RifleHolder");
                break;
            case "shotgun_name":
                playerLevelManager.UpdateWeapon("ShotgunHolder");
                break;
            case "grenade_launcher_name":
                playerLevelManager.UpdateWeapon("GrenadelauncherHolder");
                break;
            case "sniper_name":
                playerLevelManager.UpdateWeapon("SniperHolder");
                break;
            default:
                Debug.LogWarning($"Unknown weapon: {weaponName}");
                break;
        }
    }

    private void ApplySkill(string ability, float effectValue)
    {
        
        if (playerLevelManager == null) { Debug.LogError("PlayerLevelManager is null in ApplySkill."); return; }
        switch (ability)
        {
            case "max_health":
                playerLevelManager.UpdateHealthModifier(effectValue);
                break;
            case "speed":
                playerLevelManager.UpdateSpeedModifier(effectValue);
                break;
            case "damage":
                playerLevelManager.UpdateDamageModifier(effectValue);
                break;
            case "fire_rate":
                playerLevelManager.UpdateFireRateModifier(effectValue);
                break;
            case "dodge":
                playerLevelManager.UpdateDodgeChanceModifier(effectValue);
                break;
            case "max_ammo":
                playerLevelManager.UpdateMaxAmmoModifier(effectValue);
                break;
            default:
                Debug.LogWarning($"Unknown ability: {ability}");
                break;
        }
    }

    public void NextLevel()
    {
        
        if (gameManager == null) { Debug.LogError("GameManager is null in NextLevel."); return; }
        Debug.Log("Next Level");
        Debug.Log($"level{gameManager.level + 1}");
        SceneManager.LoadScene($"level{gameManager.level + 1}");
    }
}