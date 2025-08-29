using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;


[CreateAssetMenu(menuName = "Combat/Attack/SkeletonAttack")]
public class SkeletonEnemyAttack : Attack
{


    [SerializeField] public float coneRadiusVal;


    public float ConeRadiusVAL => coneRadiusVal;

    public override void Execute(GameObject attacker, GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(GameObject attacker)
    {
        Collider[] hitColliders = Physics.OverlapSphere(
           attacker.transform.position + Vector3.forward * rangeValue * 0.5f,
           rangeValue
          );

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                var player = col.GetComponent<IDamageable>();
                float finalDamage = getFinalDamage(damageValue.amount);
                player.TakeDamage(new Damage(finalDamage, DamageType.Physical, attacker, attacker.transform.position - col.gameObject.transform.position, damageValue.knockBackForce, damageValue.staggerDuration));
                break;
            }
        }
    }


}
