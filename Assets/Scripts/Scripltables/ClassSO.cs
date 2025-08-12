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
    public float damage;
    public float range;
    public bool isRanged;
    public float cooldownOfAttack;
    public float angleOfAttack;
    public float radiusOfRangedAttack;
    public float kncokback;

    public float staggerDur;

    public SkillSO minorSkill;
    public SkillSO ulty;

    public SkillSO passive;

    public void SetPlayerClass(PlayerClass playerClass) => this.playerClass = playerClass;
    public void SetDamage(float damage) => this.damage = damage;
    public void SetRange(float range) => this.range = range;
    public void SetIsRanged(bool isRanged) => this.isRanged = isRanged;
    public void SetCooldownOfAttack(float cooldownOfAttack) => this.cooldownOfAttack = cooldownOfAttack;
    public void SetAngleOfAttack(float angleOfAttack) => this.angleOfAttack = angleOfAttack;
    public void SetRadiusOfRangedAttack(float radiusOfRangedAttack) => this.radiusOfRangedAttack = radiusOfRangedAttack;
    public void SetForceOfAttack(float forceOfAttack) => this.kncokback = forceOfAttack;

    public void SetStaggerDur(float staggerDur) => this.staggerDur = staggerDur;

    
}