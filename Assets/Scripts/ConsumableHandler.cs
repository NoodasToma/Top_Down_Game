using System.Collections;
using UnityEngine;

public class ConsumableHandler : MonoBehaviour
{
 

    private float originalDamageMultiplier = 1f;
    private float originalDamageTakenMultiplier = 1f;

    void Start()
    {
        
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
            StatsManager.damateDealtMultipliers.Add(item.damageMultiplier);
        }

        if (item.boostDefense)
        {
           StatsManager.defenceMultipliers.Add(item.defenseMultiplier);
        }

        // Optional sound
        if (item.consumeSound)
            AudioSource.PlayClipAtPoint(item.consumeSound, transform.position);

        yield return new WaitForSeconds(item.duration);

        // Revert to original values
        if (item.boostDamage)
            StatsManager.damateDealtMultipliers.Remove(item.damageMultiplier);

        if (item.boostDefense)
            StatsManager.defenceMultipliers.Remove(item.defenseMultiplier);
    }
}
