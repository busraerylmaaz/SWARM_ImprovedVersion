using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] public float _attackDistanceModifier = 1.0f;
    [SerializeField] public float _attackDamageModifier = 1.0f;
    [SerializeField] public float _healthModifier = 1.0f;
    [SerializeField] public float _speedModifier = 1.0f;
    private IDamageable npcComponent;

    private void Awake()
    {
        npcComponent = GetComponent<NPC>() as IDamageable;

        if (npcComponent == null)
        {
            npcComponent = GetComponent<RangedNPC>() as IDamageable;
        }

        if (npcComponent == null)
        {
            Debug.LogError("Neither NPC nor RangedNPC component found on the GameObject.");
        }
    }
    private void Start()
    {

        npcComponent.UpdateNPC(_healthModifier, _speedModifier, _attackDistanceModifier, _attackDamageModifier);
    }

    public void UpdateNPC(float healthModifier, float speedModifier, float attackDistanceModifier, float attackDamageModifier)
    {
        _healthModifier = healthModifier;
        _speedModifier = speedModifier;
        _attackDistanceModifier = attackDistanceModifier;
        _attackDamageModifier = attackDamageModifier;
        npcComponent.UpdateNPC(_healthModifier, _speedModifier, _attackDistanceModifier, _attackDamageModifier);
    }
}
