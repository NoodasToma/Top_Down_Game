using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Combat;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Combat/Skills/Parry")]
public class Parry : SkillSO
{
    public float parryWindow;
    public static float staggerDuration = 3f;

    public float damageMultiplier;

    private static StatsManager statsManager;



    public override void OnStart(GameObject caster, Vector3 aim, Damage damage)
    {
        statsManager = caster.GetComponent<StatsManager>();
        caster.GetComponent<SkillManager>().StartCoroutine(parrying(caster));
        Debug.Log("Parryed1");
    }

    IEnumerator parrying(GameObject caster)
    {
        Debug.Log("Parryed" + "  " + parryWindow);

        statsManager.currentState = StatsManager.STATE.Parrying;

        Renderer ren = caster.GetComponentInChildren<Renderer>();
        Color originalColor = ren.material.color;
        ren.material.color = Color.blue;
        yield return new WaitForSeconds(parryWindow);
        ren.material.color = originalColor;
        statsManager.currentState = StatsManager.STATE.Basic;



    }
    public static void doParry(Damage damage , Vector3 lookDir)
    {
        if (Vector3.Angle(lookDir, damage.direction.normalized) < 45) damage.source.GetComponent<Enemy_Movement>().TakeDamage(new Damage(0, 1f, staggerDuration));
        else
        {
            statsManager.currentState = StatsManager.STATE.Basic;
            statsManager.TakeDamage(damage);
        }
    }

    
}
