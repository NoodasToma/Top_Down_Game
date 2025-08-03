

public class PlayerClassStats
{
    public PlayerClass playerClass;
    public float damage;

    public float range;

    public bool isRanged;

    public float cooldownOfAttack;
    public float angleOfAttack;

    public float radiusOfRangedAttack;

    public float forceOfAttack;

    public float skillCdMinor;

    public PlayerClassStats(PlayerClass playerClass, float damage, float range, bool isRanged, float cooldownOfAttack, float angleOfAttack, float radiusOfRangedAttack, float forceOfAttack, float skillCdMinor)
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

    public PlayerClassStats()
    {
        
    }


     // Setters
    public void SetPlayerClass(PlayerClass playerClass) => this.playerClass = playerClass;
    public void SetDamage(float damage) => this.damage = damage;
    public void SetRange(float range) => this.range = range;
    public void SetIsRanged(bool isRanged) => this.isRanged = isRanged;
    public void SetCooldownOfAttack(float cooldownOfAttack) => this.cooldownOfAttack = cooldownOfAttack;
    public void SetAngleOfAttack(float angleOfAttack) => this.angleOfAttack = angleOfAttack;
    public void SetRadiusOfRangedAttack(float radiusOfRangedAttack) => this.radiusOfRangedAttack = radiusOfRangedAttack;
    public void SetForceOfAttack(float forceOfAttack) => this.forceOfAttack = forceOfAttack;
    public void SetSkillCdMinor(float skillCdMinor) => this.skillCdMinor = skillCdMinor;

    
}