using UnityEngine;

public enum AttackType
{
    None,
    Slash,
    Fireball,
}

[CreateAssetMenu(menuName = "RPG/Player Class")]
public class PlayerClassSO : ScriptableObject
{
    public string className;
    public int maxHealth;
    public float moveSpeed;


    public AttackType primaryAttack;
}
