using System.Collections;
using UnityEngine;

public class GrenadeBullet : MonoBehaviour
{
    private AudioSource _audioSource;
    public float explosionDamage = 50f;
    public float baseDamage = 50f;
    public float explosionRadius = 1.8f;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float travelTime;
    private float timer;
    private float maxLobHeight;
    public int TTL = 5;
    private bool isLaunched = false;

    [SerializeField] private LayerMask damageableLayer;

    public void Launch(Vector2 start, Vector2 target, float speed, float lobHeight)
    {
        startPosition = start;
        targetPosition = target;
        maxLobHeight = lobHeight;

        float distance = Vector2.Distance(startPosition, targetPosition);
        travelTime = distance / speed;

        timer = 0f;
        isLaunched = true;
    }

    void Update()
    {
        if (isLaunched)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / travelTime);
            Vector2 horizontalPosition = Vector2.Lerp(startPosition, targetPosition, progress);

            float distance = Vector2.Distance(startPosition, targetPosition);
            float adjustedMaxLobHeight = maxLobHeight * (distance / (distance + 1));
            float height = Mathf.Sin(progress * Mathf.PI) * adjustedMaxLobHeight;

            transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, 0);

            if (progress >= 1f)
            {
                OnHitGround();
            }
        }
    }

    public void modifyDamage(float damageModifier)
    {
        explosionDamage = baseDamage * damageModifier;
    }

    private void OnHitGround()
    {
        isLaunched = false;
        Explode();
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        ExplosionEffect();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            damageable?.OnHit(explosionDamage);
        }
    }

    private void ExplosionEffect()
    {
        GameObject explosionEffect = Instantiate(Resources.Load<GameObject>("Weapons/Explosion/GrenadeExplosion"), transform.position, Quaternion.identity);
        explosionEffect.transform.localScale = Vector3.one;

        _audioSource = GetComponent<AudioSource>();
        GameObject audioObject = new GameObject("ExplosionAudio");
        audioObject.transform.position = transform.position;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = _audioSource.clip;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        Destroy(audioObject, audioSource.clip.length);
    }
}
