using System.Collections;
using UnityEngine;

public class NPCBullet : MonoBehaviour
{
    public float baseDamage = 25f;
    public float bulletDamage = 25f;
    public int TTL = 3;
    private bool objectHit = false;

    void OnEnable()
    {
        objectHit = false;
        bulletDamage = baseDamage;
        StartCoroutine(DeactivateAfterTime());
    }

    public void SetDamage(float newDamage)
    {
        bulletDamage = newDamage;
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(TTL);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectHit) return;

        if (collision.TryGetComponent<PlayerController>(out var mainCharacter))
        {
            mainCharacter.OnHit(bulletDamage);
            objectHit = true;
            gameObject.SetActive(false);
        }
        else if (!collision.TryGetComponent<NPC>(out _) && !collision.TryGetComponent<NPCBullet>(out _))
        {
            gameObject.SetActive(false);
        }
    }
}
