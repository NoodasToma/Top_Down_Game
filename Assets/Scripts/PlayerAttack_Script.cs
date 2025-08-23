using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Combat;

public class PlayerAttack_Script : MonoBehaviour
{

    public ClassSO player;
    public LayerMask layer;

    public LayerMask wall;

    private Animator playerAnimator;

    [SerializeField]
    private GameObject fireBoltPrefab;

    public float throwingItemCD = 5f;
    private Coroutine throwRoutine;

    private float lastClickTime = 0f;

    private int comboIndex = 0;
    private bool isAttacking = false;
    private StatsManager statsManager;


    ThrowingItems throwItem;
    public itemClass itemClass;

    private bool isAimingThrow = false;

    private Coroutine attackRout;

    // Start is called before the first frame update
    void Start()
    {

        playerAnimator = GetComponent<Animator>();
        throwItem = GetComponent<ThrowingItems>();
        statsManager = GetComponent<StatsManager>();

        //Todo  at the start assign damaga range etc based on class
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(comboIndex);
        if (lastClickTime + player.attackCombo.GetComboResetTime < Time.time) comboIndex = 0;

        if (statsManager.currentState == StatsManager.STATE.Staggered) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            lastClickTime=Time.time;
            isAttacking = true;
            if(attackRout == null)attackRout=StartCoroutine(attackRoutine());
        }

        if (Input.GetKeyDown(KeyCode.G) && throwRoutine == null)
        {
            StartAiming();
        }

        if (isAimingThrow && Input.GetKeyUp(KeyCode.G))
        {
            ExecuteThrow();
        }

        if (isAimingThrow && Input.GetMouseButtonDown(1)) // Right click to cancel
        {
            CancelThrow();
        }


    }

    IEnumerator attackRoutine()
    {
   
        if (comboIndex <= player.attackCombo.GetComboLimit)
        {
            AnimationStarter(player.attackCombo.GetAttacks[comboIndex].AnimationValue);

            if (comboIndex >= player.attackCombo.GetComboLimit)
            {
                yield return new WaitForSeconds(player.attackCombo.GetCooldownBetweenCombos);
                comboIndex = 0;
            }
            else
            {
                yield return new WaitForSeconds(player.attackCombo.GetCooldownBetweenAttacks);
                comboIndex++;
            }



        }

        isAttacking = false;
        attackRout = null;

    }

    void StartAiming()
    {
        isAimingThrow = true;
        throwItem.StartAiming(itemClass);
    }

    void ExecuteThrow()
    {
        if (!isAimingThrow) return;

        throwItem.ExecuteThrow();
        isAimingThrow = false;

        // Start cooldown
        if (throwRoutine == null)
            throwRoutine = StartCoroutine(ThrowCooldown());
    }

    // void cancelRangedAimingAttack()
    // {
    //     rangedIsAiming = false;
    //     playerSkill.CancelAiming(); // Same as skill cancel
    // }

    void CancelThrow()
    {
        isAimingThrow = false;
        throwItem.CancelAiming();
    }
    IEnumerator ThrowCooldown()
    {
        yield return new WaitForSeconds(throwingItemCD);
        throwRoutine = null;
    }




    //returns the direction player is lookin
    public static Vector3 getAim()
    {
        Vector3 v = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().getDirection();
        v.y = 0;
        return v.normalized;
    }






    // void RangerAttack(Vector3 originOfattack)
    // {
    //     RaycastHit hit;
    //     RaycastHit wallhit;
    //     bool isHit = Physics.SphereCast(originOfattack, player.radiusOfRangedAttack, getAim(), out hit, player.range,layer);
    //     bool isWall = Physics.SphereCast(originOfattack, player.radiusOfRangedAttack, getAim(), out wallhit, player.range,wall);



    //     if (isHit || isWall)
    //     {

    //         if (isHit && isWall)
    //         {
    //          Debug.Log(originOfattack);
    //          Debug.Log(hit.collider.name);
    //          Debug.Log(wallhit.collider.name);
    //          if ((wallhit.transform.position - originOfattack).magnitude <= (hit.transform.position - originOfattack).magnitude) return;
    //         }

    //         if (!isHit) return;
    //         GameEventManager.CameraShake(0.05f, 0.1f);
    //         GameEventManager.freezeFrame(0.05f);
    //         Debug.Log(hit.transform.name);
    //         float finalDamage = player.damage * (statsManager != null ? statsManager.damageMultiplier : 1f);
    //         hit.transform.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(finalDamage, player.kncokback,player.staggerDur));
    //     }
    // }


    // public void sorcererAttack()
    // {
    //     Vector3 spawnPos = transform.position + Vector3.up * 1.6f + getAim() * 0.8f;
    //     GameObject fireBolt = Instantiate(fireBoltPrefab, spawnPos, Quaternion.identity);

    //     // Ignore collision with player
    //     Collider playerCollider = GetComponent<Collider>();
    //     Collider fireBoltCollider = fireBolt.GetComponent<Collider>();
    //     if (playerCollider != null && fireBoltCollider != null)
    //     {
    //         Physics.IgnoreCollision(fireBoltCollider, playerCollider);
    //     }

    //     Rigidbody rb = fireBolt.GetComponent<Rigidbody>();
    //     if (rb != null)
    //     {
    //         rb.velocity = getAim() * 55;
    //     }

    //     Destroy(fireBolt, 5f);
    // }

    public void Attack()
    {
        player.attackCombo.startCombo(gameObject, comboIndex);
        
    }

    public void AnimationStarter(String variable)
    {
        playerAnimator.SetTrigger(variable);
    }


   
}