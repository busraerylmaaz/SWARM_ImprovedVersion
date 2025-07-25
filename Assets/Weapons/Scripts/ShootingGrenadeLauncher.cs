using UnityEngine;

public class ShootingGrenadeLauncher : WeaponBase
{
    public float fireSpeed = 10f;             
    public float maxLobHeight = 2f;           
    protected override void Fire()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);

        Debug.Log("Fire Grenade");
       
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        
        GameObject grenade = poolManager.GetPooledObject(bulletPrefab);
        GrenadeBullet grenadeBullet = grenade.GetComponent<GrenadeBullet>();
        grenadeBullet.modifyDamage(damageModifier);

        if (grenade != null)
        {
            grenade.transform.position = firePoint.position;
            grenade.transform.rotation = Quaternion.identity;
            grenade.SetActive(true);

            
            GrenadeBullet grenadeScript = grenade.GetComponent<GrenadeBullet>();
            if (grenadeScript != null)
            {
                grenadeScript.Launch(firePoint.position, mousePos, fireSpeed, maxLobHeight);
            }
        }
    }

    protected override int GetBulletPoolSize(float FR)
    {
        float maxSeconds = bulletPrefab.GetComponent<GrenadeBullet>().TTL;
        int maxBulletsInSeconds = Mathf.CeilToInt(maxSeconds / FR);
        return maxBulletsInSeconds;
    }
}
