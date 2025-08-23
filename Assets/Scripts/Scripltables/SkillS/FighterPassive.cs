using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Skills/FighterPassive")]
public class FighterPassive : SkillSO
{
    private  float baseDamage ;
    private readonly float resetTime = 5f;

    float currentMod = 1f;
    StatsManager statsManager;
    Coroutine coroutine;
    bool subscribed = false;


    void Awake()
    {
        baseDamage = statsManager.damageMultiplier;
    }
    public override void Passive()
    {
        if (!subscribed)
        if (statsManager == null)
            statsManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatsManager>();

        if (!subscribed)
        {
            GameEventManager.OnEnemyKilled += OnKill;
            subscribed = true;
        }

    }

    void OnKill()
    {
        Debug.Log("Kill event received in FighterPassive!  " + resetTime);
        currentMod += 0.1f;
        Debug.Log("New damage multiplier: " + currentMod);

        statsManager.damageMultiplier =   currentMod;
        Debug.Log("New player damage: " + statsManager.damageMultiplier);

        // Restart timer — only reset if no further kills occur
        if (coroutine != null) statsManager.StopCoroutine(coroutine);
        coroutine = statsManager.StartCoroutine(ResetDamageAfterDelay());
    }

    IEnumerator ResetDamageAfterDelay()
    {
        Debug.Log("Starting reset timer...");
        yield return new WaitForSeconds(resetTime);

        Debug.Log("No kills in " + resetTime + " seconds. Resetting damage.");
        currentMod = 1f;
        statsManager.damageMultiplier = baseDamage;
        coroutine = null;
    }
}
