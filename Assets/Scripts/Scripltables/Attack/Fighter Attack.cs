using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class FighterAttack : Attack
{

    public float rangeVal;

    public float coneRadiusVal;

    public FighterAttack(Damage damage, Vector3 direction) : base(damage, direction)
    {
    }

    public FighterAttack(Damage damage, Vector3 direction, Animation animation, LayerMask layerMask, float range, float coneRadius) : base(damage, direction, animation, layerMask)
    {
        rangeVal = range;
        coneRadiusVal = coneRadius;
    }

    

    public override void Execute(GameObject attacker, GameObject target)
    {
        StatsManager statsManager = attacker.GetComponent<StatsManager>();
        Vector3 originOfattack = attacker.transform.position + PlayerAttack_Script.getAim() * 0.5f;

        Collider[] hitEnemies = Physics.OverlapSphere(originOfattack, rangeVal, layerMaskValue);
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
                c.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(finalDamage, damageValue.knockBackForce, damageValue.staggerDuration, attacker));
            }
        }
    }
}
