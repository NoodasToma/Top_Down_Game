using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumable")]
public class ConsumableSO : ScriptableObject
{
    public string itemName;
    public float duration = 60f;

    public bool boostDamage;
    public float damageMultiplier = 1.5f;

    public bool boostDefense;
    public float defenseMultiplier = 0.5f; // 50% less damage

    public AudioClip consumeSound;
    public Sprite icon;
}
