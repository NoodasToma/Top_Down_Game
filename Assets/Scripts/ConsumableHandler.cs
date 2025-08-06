using System.Collections;
using UnityEngine;

public class ConsumableHandler : MonoBehaviour
{
    private StatsManager stats;

    private float originalDamageMultiplier = 1f;
    private float originalDamageTakenMultiplier = 1f;

    void Start()
    {
        stats = GetComponent<StatsManager>();
    }

    public void Consume(ConsumableSO item)
    {
        StartCoroutine(ApplyEffect(item));
    }

    private IEnumerator ApplyEffect(ConsumableSO item)
    {
        Debug.Log($"Consumed {item.itemName}");

        if (item.boostDamage)
        {
            originalDamageMultiplier = stats.damageMultiplier;
            stats.damageMultiplier *= item.damageMultiplier;
        }

        if (item.boostDefense)
        {
            originalDamageTakenMultiplier = stats.damageTakenMultiplier;
            stats.damageTakenMultiplier *= item.defenseMultiplier;
        }

        // Optional sound
        if (item.consumeSound)
            AudioSource.PlayClipAtPoint(item.consumeSound, transform.position);

        yield return new WaitForSeconds(item.duration);

        // Revert to original values
        if (item.boostDamage)
            stats.damageMultiplier = originalDamageMultiplier;

        if (item.boostDefense)
            stats.damageTakenMultiplier = originalDamageTakenMultiplier;
    }
}
