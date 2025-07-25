using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _maxHealth = 100f;
    
    private float _health = 100f;
    
    private float _speed = 7f;
    private float _speedModifier = 1f;
    private float _dodgeChance = 0.1f;
    private float _maxAmmoModifier = 1f;
    private float _damageModifier = 1f;
    private float _fireRateModifier = 1f;
    private Rigidbody2D rb;
    private Transform tr;
    public Camera cam;

    private Animator an;
    private DamageFlash damageFlash;
    public event Action<float> OnHealthChanged;
    public GameManager gameManager;
    public event Action<bool> OnLoss;
    public event Action<bool> OnDodge;
    private float dashSpeed = 15f;
    private float dashDuration = 0.5f;
    private bool isDashing = false;
    private Vector2 dashDirection;
    public GameObject dustPrefab;
    
    private float dashCooldown = 1.5f;
    private float dashCooldownTimer = 0f; public WeaponBase weaponBase;
    private PlayerStats playerStats;

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.playerController = this;
        gameManager.OnLevelStartPlayer();

        damageFlash = GetComponent<DamageFlash>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        an = GetComponent<Animator>();
        cam = Camera.main;

        weaponBase = GetComponentInChildren<WeaponBase>();


        UpdateNPC(playerStats.maxHealthModifier, playerStats.speedModifier, playerStats.dodgeChanceModifier);
        UpdateWeapon(playerStats.damageModifier, playerStats.fireRateModifier, playerStats.maxAmmoModifier);

    }
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (!isDashing)
        {
            MovePlayer(moveX, moveY);
            AnimatePlayer(moveX, moveY);
        }

        if (Input.GetMouseButton(0))
        {
            an.SetBool("isShooting", true);
        }
        else
        {
            an.SetBool("isShooting", false);
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;

        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        dashDirection = new Vector2(moveX, moveY).normalized;

        
        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(an.GetFloat("DirectionX"), an.GetFloat("DirectionY")).normalized;
        }

        an.SetFloat("DirectionX", dashDirection.x);
        an.SetFloat("DirectionY", dashDirection.y);
        an.SetTrigger("Dash");

        SpawnDust(dashDirection);

        dashCooldownTimer = dashCooldown;

        StartCoroutine(DashCoroutine());
    }
    void SpawnDust(Vector2 direction)
    {
        if (dustPrefab != null)
        {
            GameObject dust = Instantiate(dustPrefab, tr.position, Quaternion.identity);

            Animator dustAnimator = dust.GetComponent<Animator>();
            if (dustAnimator != null)
            {
                dustAnimator.SetFloat("DirectionX", direction.x);
                dustAnimator.SetFloat("DirectionY", direction.y);
            }
        }
    }
    private System.Collections.IEnumerator DashCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            rb.velocity = dashDirection * dashSpeed * _speedModifier;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
    }
    void MovePlayer(float moveX, float moveY)
    {


        Vector2 movement = new Vector2(moveX, moveY);

        if (movement != Vector2.zero)
        {
            rb.velocity = movement.normalized * _speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void AnimatePlayer(float moveX, float moveY)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (mousePos - (Vector2)tr.position).normalized;
        an.SetFloat("DirectionX", lookDir.x);
        an.SetFloat("DirectionY", lookDir.y);
        an.SetBool("isMoving", moveX != 0 || moveY != 0);
    }

    public void OnHit(float bulletDamage)
    {

        if (UnityEngine.Random.value < _dodgeChance)
        {
            Debug.Log("Dodge");
            OnDodge?.Invoke(true);
            return;
        }

        if (damageFlash != null)
        {
            damageFlash.CallFlash();
        }
        _health -= bulletDamage;
        OnHealthChanged?.Invoke(GetHealthPercentage());

        if (_health <= 0)
        {

            OnLoss?.Invoke(true);
        }
    }

    public float GetHealthPercentage()
    {
        return _health / _maxHealth;
    }

    public void UpdateNPC(float maxHealthModifier, float speedModifier, float dodgeChanceModifier)
    {
        _maxHealth *= maxHealthModifier;
        _speed *= speedModifier;
        _speedModifier = speedModifier;
        _dodgeChance *= dodgeChanceModifier;
    }

    public void UpdateWeapon(float damageModifier, float fireRateModifier, float maxAmmoModifier)
    {
        weaponBase.SetFireRate(fireRateModifier);
        weaponBase.SetDamageModifier(damageModifier);
        weaponBase.SetMaxAmmo(maxAmmoModifier);
    }

    public void SetPlayerStats(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }
}