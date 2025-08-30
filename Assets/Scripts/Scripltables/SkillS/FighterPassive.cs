using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Skills/FighterPassive")]
public class FighterPassive : Passive
{
    private  float baseDamage ;
    private readonly float resetTime = 5f;

    float currentMod = 1f;

    Coroutine coroutine;
    bool subscribed = false;

    private int modIndex;


   
    public override void execute(GameObject caster)
    {


        if (!subscribed)
        {
            GameEventManager.OnEnemyKilled += OnKill;
            subscribed = true;
        }

    }

    

    void OnKill()
    {
        if (currentMod == 1)
        {
            currentMod += 0.1f;
            StatsManager.damateDealtMultipliers.Add(currentMod);
            modIndex = StatsManager.damateDealtMultipliers.IndexOf(currentMod);
            Debug.Log(modIndex);
        }
        else
        {
            currentMod += 0.1f;
            StatsManager.damateDealtMultipliers[modIndex] = currentMod;
        }
        Debug.Log("Kill event received in FighterPassive!  " + resetTime);
        
        Debug.Log("New damage multiplier: " + currentMod);

        

        // Restart timer — only reset if no further kills occur
        if (coroutine != null) GameEventManager.gameEventManager.StopCoroutine(coroutine);
        coroutine = GameEventManager.gameEventManager.StartCoroutine(ResetDamageAfterDelay());
    }

    IEnumerator ResetDamageAfterDelay()
    {
        Debug.Log("Starting reset timer...");
        yield return new WaitForSeconds(resetTime);

        Debug.Log("No kills in " + resetTime + " seconds. Resetting damage.");
        currentMod = 1f;
        StatsManager.damateDealtMultipliers.RemoveAt(modIndex);
        coroutine = null;
    }
}
