using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public NPCLevelManager npcLevelManager;
    public PlayerLevelManager playerLevelManager;
    public PlayerController playerController;
    public int levelWon = 0;
    public int level = 0;
    public string characterName;
    public string gunName;
    public int mobCount;
    public PlayerStats modifiers;
    public float timeRemaining;
    public float mobsKilled = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        npcLevelManager = NPCLevelManager.Instance;
        playerLevelManager = PlayerLevelManager.Instance;

        characterName = playerLevelManager.playerData.character;
        gunName = playerLevelManager.playerData.weapon;
        modifiers = playerLevelManager.playerData.stats;

        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.OnLoss += OnResultLose;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        npcLevelManager.OnWin += OnResultWin;
        npcLevelManager.OnLoss += OnResultLose;

        playerLevelManager.onWeaponChanged += UpdateWeapon;
    }

    void OnResultWin(bool result)
    {
        level = npcLevelManager.levelConfig.levelNumber;

        if (result)
        {
            levelWon = 1;
            FinishLevel();
        }
    }

    void UpdateWeapon(string weaponName)
    {
        gunName = weaponName;
    }

    void OnResultLose(bool result)
    {
        level = npcLevelManager.levelConfig.levelNumber;

        if (result)
        {
            Debug.Log("Player Lost");
            levelWon = 0;
            FinishLevel();
        }
    }

    public void OnLevelStartNPC()
    {
        levelWon = 0;
        npcLevelManager.OnWin += OnResultWin;
        npcLevelManager.OnLoss += OnResultLose;
    }

    public void OnLevelStartPlayer()
    {
        playerController.OnLoss += OnResultLose;
    }

    void FinishLevel()
    {
        mobCount = npcLevelManager.mobsKilled;
        timeRemaining = npcLevelManager.levelTimer;

        if (levelWon == 1)
        {
            int reward = CalculateReward();
            PlayerLevelManager.Instance.UpdateCurrency(reward);
            Debug.Log($"Kazan?: {reward} para eklendi. Toplam: {PlayerLevelManager.Instance.GetCurrency()}");
        }

        SceneManager.LoadScene("LevelTransitionMenu");
    }

    public int CalculateReward()
    {
        int baseReward = 25; 

        float healthMultiplier = 1f;
        float timeMultiplier = 1f;
        float killMultiplier = 1f;

        if (playerController != null)
        {
            float currentHealth = playerController.GetHealthPercentage(); 
            healthMultiplier = Mathf.Clamp01(currentHealth);
        }

        float totalLevelTime = npcLevelManager.levelConfig.levelDuration;
        if (totalLevelTime > 0)
            timeMultiplier = Mathf.Clamp01(timeRemaining / totalLevelTime);

        if (npcLevelManager.mobCount > 0)
            killMultiplier = Mathf.Clamp01(mobCount / (float)npcLevelManager.mobCount);

       
        float performanceScore = (healthMultiplier + timeMultiplier + killMultiplier) / 3f;

        
        float performanceMultiplier = Mathf.Lerp(0.5f, 1.5f, performanceScore);

        int totalReward = Mathf.RoundToInt(baseReward * performanceMultiplier);
        return totalReward;
    }

}
