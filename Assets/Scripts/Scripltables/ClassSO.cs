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
    
    public Combo attackCombo; // in case of single attacks combo will consist of leght 1 

    public SkillSO minorSkill;
    public SkillSO ulty;

    public SkillSO passive;

    
}