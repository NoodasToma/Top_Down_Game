using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public class StatsManager : MonoBehaviour, IDamageable, IKillable
{
    Ui_script ui_Script;
    public Animator playerAnimator;

    public bool alive = true;
    private Material _originalMaterial;
    public ClassSO classs;
    [Header("Core Stats")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("Movement Stats")]
    public float moveSpeed = 5f;
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.25f;
    public float dodgeCooldown = 1f;
    public float iFrameDuration = 0.15f;

    [Header("Combat Modifiers")]
    public float damageMultiplier = 1f;
    public float damageTakenMultiplier = 1f;




    public enum STATE
    {
        Basic, Dodging

    }

    public STATE currentState;
    // Start is called before the first frame update
    void Start()
    {
        ui_Script = GetComponent<Ui_script>();
        if (ui_Script == null)
            ui_Script = GameObject.FindGameObjectWithTag("HpBar")?.GetComponent<Ui_script>();
        currentState = STATE.Basic;
        playerAnimator = GetComponent<Animator>();
        currentHP = maxHP;
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            _originalMaterial = new Material(renderer.material); // Create a copy
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(Damage damage)
    {
        if (!alive) return;
        if (currentState == STATE.Dodging) return;

        Character_Passives passives = GetComponent<Character_Passives>();
        PlayerAttack_Script attackScript = GetComponent<PlayerAttack_Script>();

        if (passives != null && attackScript != null && attackScript.pClass == PlayerClass.Sorcerer)
        {
            // Let Character_Passives handle damage and cheat death for Sorcerer
            passives.CheckForCheatDeath(damage.amount);
            return; // Exit so damage is handled only in CheckForCheatDeath
        }


        currentHP -= damage.amount * damageTakenMultiplier;;

        // Visual feedback (flash effect)
        // Visual feedback (flash effect)
        StartCoroutine(DamageFlash());

        if (currentHP <= 0 && alive) death(damage.source);
        ui_Script.setHpBar(currentHP);
    }

    public void death(GameObject killer)
    {
        currentHP = 0;
        alive = false;
        ui_Script.gameOver();
        GameObject.Destroy(gameObject.GetComponent<PlayerAttack_Script>());
        playerAnimator.SetTrigger("Dead");        
    }
    IEnumerator DamageFlash()
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && _originalMaterial != null)
        {
            // Create a temporary material instance for flashing
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            // Restore the original material properties
            renderer.material.CopyPropertiesFromMaterial(_originalMaterial);
        }
    }
}
