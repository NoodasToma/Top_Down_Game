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

    public float RecoveryDuration = 1f;




    public enum STATE
    {
        Basic, Dodging, Staggered, Recovering

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
        if (currentState == STATE.Dodging||currentState==STATE.Staggered) return;

        Character_Passives passives = GetComponent<Character_Passives>();
        PlayerAttack_Script attackScript = GetComponent<PlayerAttack_Script>();

        if (passives != null && attackScript != null && classs.playerClass == PlayerClass.Sorcerer)
        {
            // Let Character_Passives handle damage and cheat death for Sorcerer
            passives.CheckForCheatDeath(damage.amount);
            StartCoroutine(StaggerRoutine(damage.knockBackForce,damage.staggerDuration,calcStaggerDir(damage.source)));
            return; // Exit so damage is handled only in CheckForCheatDeath
        }


        currentHP -= damage.amount * damageTakenMultiplier;;

        // Visual feedback (flash effect)
        // Visual feedback (flash effect)
        if (currentHP <= 0 && alive) death(damage.source);
        ui_Script.setHpBar(currentHP);

       
        
        StartCoroutine(StaggerRoutine(damage.knockBackForce,damage.staggerDuration,calcStaggerDir(damage.source)));
    }
    
     private Vector3 calcStaggerDir(GameObject source)
    {
        return ((transform.position - source.transform.position)).normalized;
    }

    public void death(GameObject killer)
    {
        currentHP = 0;
        alive = false;
        ui_Script.gameOver();
        GameObject.Destroy(gameObject.GetComponent<PlayerAttack_Script>());
        playerAnimator.SetTrigger("Dead");
    }

    //staggers player and knocks him back 
  IEnumerator StaggerRoutine(float knockBackDis,float staggerDuration, Vector3 knockBackDir)
    {
        Debug.Log("RoutineStarted");
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && _originalMaterial != null)
        {
            // Create a temporary material instance for flashing
            renderer.material.color = Color.red;
        }

        if (currentState != STATE.Recovering)
        {
            currentState = STATE.Staggered;
            knockBackDir.Set(knockBackDir.x, 0, knockBackDir.z);
            transform.position += knockBackDir*knockBackDis; // Vaska do this shi with rirg body idk how to dew it
            yield return new WaitForSeconds(staggerDuration);

            
             renderer.material.color = Color.green;
            
            currentState = STATE.Recovering;
            yield return new WaitForSeconds(RecoveryDuration);
            if (renderer != null && _originalMaterial != null)
            {
                renderer.material.CopyPropertiesFromMaterial(_originalMaterial);
            }
            currentState = STATE.Basic;
        }

        
        
    }
}
