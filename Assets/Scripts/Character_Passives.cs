using System.Collections;
using UnityEngine;
using Combat;
public class Character_Passives : MonoBehaviour
{
    private Player_Movement movementScript;
    private PlayerAttack_Script attackScript;
    Ui_script ui_Script;

    public GameObject explosionPrefab; // Assign in Inspector (big explosion effect)
    private bool hasCheatedDeath = false;

    void Start()
    {
        movementScript = GetComponent<Player_Movement>();
        attackScript = GetComponent<PlayerAttack_Script>();
        ui_Script = GetComponent<Ui_script>();

        movementScript = GetComponent<Player_Movement>();
        attackScript = GetComponent<PlayerAttack_Script>();

        // If Ui_script is on the same GameObject:
        ui_Script = GetComponent<Ui_script>();

        // Or if Ui_script is on another GameObject with tag "HpBar":
        if (ui_Script == null)
            ui_Script = GameObject.FindGameObjectWithTag("HpBar")?.GetComponent<Ui_script>();

        if (movementScript == null)
            Debug.LogError("Character_Passives: Player_Movement component missing!");

        if (attackScript == null)
            Debug.LogError("Character_Passives: PlayerAttack_Script component missing!");

        if (ui_Script == null)
            Debug.LogError("Character_Passives: Ui_script component missing!");



    }

    public void CheckForCheatDeath(float damage)
    {
        if (attackScript == null || attackScript.Class != playerClass.Sorcerer)
        {
            // Not a sorcerer, apply normal damage
            ApplyNormalDamage(damage);
            return;
        }

        float projectedHP = movementScript.playerHP - damage;

        if (!hasCheatedDeath && projectedHP <= 0 && movementScript.alive)
        {
            // Activate Cheat Death
            hasCheatedDeath = true;
            movementScript.playerHP = 1f;
            ui_Script.setHpBar(1f);
            StartCoroutine(TriggerExplosion());
            return; // Do NOT apply damage, Cheat Death saved you
        }

        // Otherwise apply damage normally
        ApplyNormalDamage(damage);
    }

    private void ApplyNormalDamage(float damage)
    {
        movementScript.playerHP -= damage;
        ui_Script.setHpBar(movementScript.playerHP);

        if (movementScript.playerHP <= 0 && movementScript.alive)
        {
            movementScript.playerHP = 0;
            movementScript.alive = false;
            ui_Script.gameOver();
            GameObject.Destroy(movementScript.gameObject.GetComponent<PlayerAttack_Script>());
            movementScript.playerAnimator.SetTrigger("Dead");
        }
    }


    IEnumerator TriggerExplosion()
    {
        // Instantiate the explosion visual effect
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Optional: Add explosion logic (e.g., damage nearby enemies)
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 6f);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<IDamageable>()?.TakeDamage(new Damage(100f, 0f)); // Arbitrary explosion damage
            }
        }

        yield return null;
    }
    
}
