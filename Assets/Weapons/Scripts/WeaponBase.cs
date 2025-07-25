using UnityEngine;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    protected AudioManager audioManager;
    protected AudioSource audioSource;
    public float baseVolume = 0.7f;
    public Transform firePoint;
    public float reloadTime = 2f;
    public event Action<float> OnReloadStarted;
    public event Action<int, int> OnAmmoChange;
    public float fireRate = 1.0f;
    protected float nextFireTime = 0f;
    protected float damageModifier = 1f;
    public int ammo = 0;
    public int maxAmmo = 15;
    public ObjectPool poolManager;
    public GameObject bulletPrefab;
    public Camera cam;
    private bool reloading = false;

    protected abstract void Fire();

    void Awake()
    {
        
        if (poolManager == null)
        {
            poolManager = FindObjectOfType<ObjectPool>();
            if (poolManager == null)
                Debug.LogError("No ObjectPool found in the scene.");
        }

       
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
                Debug.LogError("No MainCamera found.");
        }

        ammo = maxAmmo;
        OnAmmoChange?.Invoke(ammo, maxAmmo);

        audioManager = AudioManager.Instance;
    }

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            Debug.LogError("WeaponBase: AudioSource missing.");

        poolManager.IncreasePoolSize(bulletPrefab, GetBulletPoolSize(fireRate));

        ammo = maxAmmo;
        OnAmmoChange?.Invoke(ammo, maxAmmo);
    }

    public (int, int) GetAmmo()
    {
        return (ammo, maxAmmo);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (ammo > 0 && !reloading)
            {
                Fire();
                ammo--;
                OnAmmoChange?.Invoke(ammo, maxAmmo);
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                Reload();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
        {
            Reload();
        }
    }

    private void Reload()
    {
        if (ammo == maxAmmo) return;

        OnReloadStarted?.Invoke(reloadTime);
        StartCoroutine(ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        OnAmmoChange?.Invoke(ammo, maxAmmo);
        reloading = false;
    }

    public virtual void SetFireRate(float fireRateModifier)
    {
        float oldFireRate = fireRate;
        fireRate *= fireRateModifier;

        int poolSizeDifference = GetBulletPoolSize(oldFireRate) - GetBulletPoolSize(fireRate);

        if (poolSizeDifference > 0)
            poolManager.IncreasePoolSize(bulletPrefab, poolSizeDifference);
        else if (poolSizeDifference < 0)
            poolManager.DecreasePoolSize(bulletPrefab, -poolSizeDifference);
    }

    public void SetDamageModifier(float newDamageModifier)
    {
        damageModifier = newDamageModifier;
    }

    public void SetMaxAmmo(float maxAmmoModifier)
    {
        maxAmmo = (int)(maxAmmo * maxAmmoModifier);
        ammo = maxAmmo;
        OnAmmoChange?.Invoke(ammo, maxAmmo);
    }

    protected virtual int GetBulletPoolSize(float FR)
    {
        float maxSeconds = bulletPrefab.GetComponent<Bullet>().TTL;
        return Mathf.CeilToInt(maxSeconds / FR);
    }

    
    protected void PlaySfx(AudioClip clip)
    {
        if (clip == null || audioSource == null || audioManager == null) return;

        float finalVolume = audioManager.GetSfxVolume() * baseVolume;
        audioSource.PlayOneShot(clip, finalVolume);
    }
}
