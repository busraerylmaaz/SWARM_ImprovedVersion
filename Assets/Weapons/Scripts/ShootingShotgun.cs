using UnityEngine;

public class ShootingShotgun : WeaponBase
{
    public float bulletForce = 35f;
    public int numberOfPellets = 7; 
    public float spreadAngle = 30f; 
    protected override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        poolManager.IncreasePoolSize(bulletPrefab, GetBulletPoolSize(fireRate, numberOfPellets));
        ammo = maxAmmo;
    }
    public override void SetFireRate(float newFireRate)
    {
        float oldFireRate = fireRate;
        fireRate = newFireRate;

        int poolSizeDifference = GetBulletPoolSize(oldFireRate, numberOfPellets) - GetBulletPoolSize(fireRate, numberOfPellets);

        if (poolSizeDifference > 0)
        {
            poolManager.IncreasePoolSize(bulletPrefab, poolSizeDifference);
        }
        else if (poolSizeDifference < 0)
        {
            poolManager.DecreasePoolSize(bulletPrefab, -poolSizeDifference);
        }
    }

    protected int GetBulletPoolSize(float FR, int pelletsPerShot)
    {
        float maxSeconds = bulletPrefab.GetComponent<ShotgunBullet>().TTL;
        int maxBulletsInSeconds = Mathf.CeilToInt(maxSeconds / FR) * pelletsPerShot;
        return maxBulletsInSeconds;
    }

    // void Update()
    // {
    //     if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
    //     {
    //         Fire();
    //         nextFireTime = Time.time + fireRate;
    //     }
    // }

    protected override void Fire()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - (Vector2)firePoint.position;
        float baseAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        float halfSpread = spreadAngle / 2f; 
        float angleStep = spreadAngle / (numberOfPellets - 1); 

        for (int i = 0; i < numberOfPellets; i++)
        {
            float angle = baseAngle - halfSpread + (i * angleStep); 
            ShootBullet(angle);
        }
    }

    void ShootBullet(float angle)
    {
        GameObject bullet = poolManager.GetPooledObject(bulletPrefab);
        if (bullet != null)
        {
            // Set bullet's position and rotation
            bullet.transform.SetPositionAndRotation(firePoint.position, Quaternion.Euler(0, 0, angle - 90f));
            bullet.SetActive(true);
            bullet.GetComponent<ShotgunBullet>().modifyDamage(damageModifier);
            // Apply velocity to the bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rb.velocity = direction.normalized * bulletForce;
        }
    }
}
