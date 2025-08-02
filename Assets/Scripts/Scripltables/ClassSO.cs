using UnityEngine;
[CreateAssetMenu(menuName = "Combat/Class")]
public class ClassSO : ScriptableObject
{
    public float damage;
    public float range;
    public bool isRanged;
    public float cooldownOfAttack;
    public float angleOfAttack;
    public float radiusOfRangedAttack;

    public float kncokback;

    public float skillCdMinor;
}