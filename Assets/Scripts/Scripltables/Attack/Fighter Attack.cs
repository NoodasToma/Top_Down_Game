using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Combat/Attack/fighterAttack")]
public class FighterAttack : Attack
{


    [SerializeField] public float coneRadiusVal;


    public float ConeRadiusVAL => coneRadiusVal;




    public override void Execute(GameObject attacker)
    {
        StatsManager statsManager = attacker.GetComponent<StatsManager>();
        Vector3 originOfattack = attacker.transform.position + PlayerAttack_Script.getAim() * 0.5f;

        Collider[] hitEnemies = Physics.OverlapSphere(originOfattack, rangeValue, layerMaskValue);
        if (hitEnemies.Length <= 0) return;
        GameEventManager.CameraShake(0.1f, 0.2f);
        GameEventManager.freezeFrame(0.05f);
        foreach (Collider c in hitEnemies)
        {
            Vector3 positionEnemy = c.transform.position - attacker.transform.position;
            positionEnemy.y = 0;
            positionEnemy = positionEnemy.normalized;
            float angle = Vector3.Angle(PlayerAttack_Script.getAim(), positionEnemy);
            if (angle <= coneRadiusVal && c.gameObject != null)
            {
                float finalDamage = damageValue.amount * (statsManager != null ? statsManager.damageMultiplier : 1f);
                Debug.Log("knockedBack   " + damageValue.knockBackForce);
                c.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(finalDamage, damageValue.knockBackForce, damageValue.staggerDuration, attacker));
            }
        }
    }

    public override void Execute(GameObject attacker, GameObject target)
    {
        throw new NotImplementedException();
    }

    void OnDrawGizmosSelected()
    {
        GameObject attacker = GameObject.FindGameObjectWithTag("Player");
        Vector3 hitBoxOrigin = attacker.transform.position + PlayerAttack_Script.getAim() * 0.5f;


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attacker.transform.position, rangeValue); // Match radius

        Vector3 center = PlayerAttack_Script.getAim();
        Gizmos.color = Color.green;
        Gizmos.DrawRay(hitBoxOrigin, center * rangeValue);

        Vector3 left = Quaternion.Euler(0, coneRadiusVal, 0) * center;
        Vector3 right = Quaternion.Euler(0, -coneRadiusVal, 0) * center;

        Gizmos.DrawRay(hitBoxOrigin, left * rangeValue);
        Gizmos.DrawRay(hitBoxOrigin, right * rangeValue);

    }
     


}
