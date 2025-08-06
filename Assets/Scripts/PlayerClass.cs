public enum playerClass
{

    Fighter, Sorcerer, Rogue, Warlock, Alchemist, Ranger
}

public class playerClassStats
{
    public playerClass playerClass;
    public float damage;

    public float range;

    public bool isRanged;

    public float cooldownOfAttack;
    public float angleOfAttack;

    public float radiusOfRangedAttack;

    public float forceOfAttack;

    public float skillCdMinor;



    public playerClassStats(playerClass playerClass, float damage, float range, bool isRanged, float cooldownOfAttack, float angleOfAttack, float radiusOfRangedAttack, float forceOfAttack, float skillCdMinor)
    {
        this.playerClass = playerClass;
        this.damage = damage;
        this.range = range;
        this.isRanged = isRanged;
        this.cooldownOfAttack = cooldownOfAttack;
        this.angleOfAttack = angleOfAttack;
        this.radiusOfRangedAttack = radiusOfRangedAttack;
        this.forceOfAttack = forceOfAttack;
        this.skillCdMinor = skillCdMinor;

    }

    public playerClassStats()
    {
        
    }


     // Setters
    public void SetPlayerClass(playerClass playerClass) => this.playerClass = playerClass;
    public void SetDamage(float damage) => this.damage = damage;
    public void SetRange(float range) => this.range = range;
    public void SetIsRanged(bool isRanged) => this.isRanged = isRanged;
    public void SetCooldownOfAttack(float cooldownOfAttack) => this.cooldownOfAttack = cooldownOfAttack;
    public void SetAngleOfAttack(float angleOfAttack) => this.angleOfAttack = angleOfAttack;
    public void SetRadiusOfRangedAttack(float radiusOfRangedAttack) => this.radiusOfRangedAttack = radiusOfRangedAttack;
    public void SetForceOfAttack(float forceOfAttack) => this.forceOfAttack = forceOfAttack;
    public void SetSkillCdMinor(float skillCdMinor) => this.skillCdMinor = skillCdMinor;

    
}