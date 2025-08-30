using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Combat;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Combat/Skills/Parry")]
public class Parry : MinorSkill
{
    public float parryWindow;
    public static float staggerDuration = 3f;

    public static float damageMultiplier = 1;

    private static StatsManager statsManager;



    

    IEnumerator parrying(GameObject caster)
    {
        statsManager.currentState = StatsManager.STATE.Parrying;
        yield return new WaitForSeconds(parryWindow);
        statsManager.currentState = StatsManager.STATE.Basic;
    }
    

    public static void doParry(Damage damage, Vector3 lookDir)
    {
        if (Vector3.Angle(lookDir, damage.direction.normalized) <= 90)
        {
            if (Vector3.Distance(damage.source.transform.position, statsManager.gameObject.transform.position) < 5)
            {
                damage.source.GetComponent<Enemy_Movement>().TakeDamage(new Damage(0, 1f, staggerDuration));
                damage.source.GetComponent<Enemy_Movement>().enemyState = Enemy_Movement.ENEMY_STATE.Parried;
            }
        }
        else
        {
            statsManager.currentState = StatsManager.STATE.Basic;
            statsManager.TakeDamage(damage);
        }
    }

    

    public override void skill(GameObject caster)
    {
        statsManager = caster.GetComponent<StatsManager>();
        GameEventManager.gameEventManager.StartCoroutine(parrying(caster));
    }
}
