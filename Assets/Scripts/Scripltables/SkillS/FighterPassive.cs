using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Skills/FighterPassive")]
public class FighterPassive : SkillSO
{
    private readonly float baseDamage = 10f;
    private readonly float resetTime = 5f;

    float currentMod = 1f;
    PlayerAttack_Script playerAttack_Script;
    Coroutine coroutine;
    bool subscribed = false;

    public override void Passive()
    {
        if (!subscribed) Debug.Log("Subscribing to kill event");
        if (playerAttack_Script == null)
            playerAttack_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack_Script>();

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

        playerAttack_Script.test_damage = baseDamage * currentMod;
        Debug.Log("New player damage: " + playerAttack_Script.test_damage);

        // Restart timer — only reset if no further kills occur
        if (coroutine != null) playerAttack_Script.StopCoroutine(coroutine);
        coroutine = playerAttack_Script.StartCoroutine(ResetDamageAfterDelay());
    }

    IEnumerator ResetDamageAfterDelay()
    {
        Debug.Log("Starting reset timer...");
        yield return new WaitForSeconds(resetTime);

        Debug.Log("No kills in " + resetTime + " seconds. Resetting damage.");
        currentMod = 1f;
        playerAttack_Script.test_damage = baseDamage;
        coroutine = null;
    }
}
