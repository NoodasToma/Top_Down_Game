using Combat;
using UnityEngine;
[System.Serializable]
public enum PlayerClass
{
    Fighter, Sorcerer, Rogue, Warlock, Alchemist, Ranger
}
[CreateAssetMenu(menuName = "Combat/Class")]
public class ClassSO : ScriptableObject
{
    [SerializeField]
    public PlayerClass playerClass;

    SkillManager skillManager;
    
    public float cdBetweenAttacks;
    
    public Attack attack;

    public SkillSO minorSkill;
    public SkillSO ulty;

    public SkillSO passive;

    
}