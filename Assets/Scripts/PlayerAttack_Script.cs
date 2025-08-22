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

    private Coroutine attackRoutine;


    [SerializeField]
    private GameObject fireBoltPrefab;

    public float throwingItemCD = 5f;
    private Coroutine throwRoutine;

    private float lastClickTime = 0f;
    private bool isAttacking = false;
    private StatsManager statsManager;


    ThrowingItems throwItem;
    public itemClass itemClass;

    private bool isAimingThrow = false;

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
        if (statsManager.currentState == StatsManager.STATE.Staggered) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            playerAnimator.SetTrigger(player.attack.AnimationValue);
            isAttacking = true;
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


    // IEnumerator swing()  // coroutine that manages attack cooldowns
    // {

    //     Debug.Log("swong");
    //     isAttacking = true;

    //     float speedTemp = GetComponent<Player_Movement>().speed;
    //     if (!player.isRanged) GetComponent<Player_Movement>().speed = speedTemp / 4;


    //     switch (comboIndex)
    //     {
    //         case 0: playerAnimator.SetTrigger("Attack"); break;
    //         case 1: playerAnimator.SetTrigger("Fireball"); break;
    //         case 2: playerAnimator.SetTrigger("Attack"); break;
    //     }


    //     // Vector3 push = getAim() * 1f;
    //     // transform.position += push;


    //     yield return new WaitForSeconds(player.cooldownOfAttack);

    //     basicAttack(player.playerClass);

    //     comboIndex++;

    //     if (comboIndex > 2)
    //     {
    //         comboIndex = 0;
    //         comboOnCd = true;
    //         yield return new WaitForSeconds(comboCd);  // Cooldown before new combo
    //         comboOnCd = false;
    //     }

    //     isAttacking = false;

    //     GetComponent<Player_Movement>().speed = speedTemp;


    //     attackRoutine = null;
    // }

    //returns the direction player is lookin
    public static Vector3 getAim()
    {
        Vector3 v = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().getDirection();
        v.y = 0;
        return v.normalized;
    }




    // void basicAttack(PlayerClass playerClass)
    // {
    //     Vector3 hitBoxOrigin = transform.position + getAim() * 0.5f;
    //     switch (playerClass)
    //     {
    //         case PlayerClass.Sorcerer:
    //             sorcererAttack();
    //             break;
    //         case PlayerClass.Fighter:
    //             FighterAttack(hitBoxOrigin);
    //             break;
    //         case PlayerClass.Rogue:
    //             //Todo
    //             break;
    //         case PlayerClass.Ranger:
    //             RangerAttack(hitBoxOrigin);
    //             break;
    //         case PlayerClass.Alchemist:
    //             //Todo
    //             break;
    //         case PlayerClass.Warlock:
    //             //Todo
    //             break;
    //         default:

    //             break;
    //     }
    // }

    // void FighterAttack(Vector3 originOfattack)
    // {
    //     Debug.Log("Attacked");

    //     Collider[] hitEnemies = Physics.OverlapSphere(originOfattack, player.range, layer);
    //     if (hitEnemies.Length <= 0) return;
    //     GameEventManager.CameraShake(0.1f, 0.2f);
    //     GameEventManager.freezeFrame(0.05f);
    //     foreach (Collider c in hitEnemies)
    //     {
    //         Vector3 positionEnemy = c.transform.position - transform.position;
    //         positionEnemy.y = 0;
    //         positionEnemy = positionEnemy.normalized;
    //         float angle = Vector3.Angle(getAim(), positionEnemy);
    //         if (angle <= player.angleOfAttack && c.gameObject != null)
    //         {
    //             float finalDamage = player.damage * (statsManager != null ? statsManager.damageMultiplier : 1f);
    //             c.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(finalDamage, player.kncokback,player.staggerDur,gameObject));
    //         }
    //     }
    // }

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
    public void sorcererAttack()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 1.6f + getAim() * 0.8f;
        GameObject fireBolt = Instantiate(fireBoltPrefab, spawnPos, Quaternion.identity);

        // Ignore collision with player
        Collider playerCollider = GetComponent<Collider>();
        Collider fireBoltCollider = fireBolt.GetComponent<Collider>();
        if (playerCollider != null && fireBoltCollider != null)
        {
            Physics.IgnoreCollision(fireBoltCollider, playerCollider);
        }

        Rigidbody rb = fireBolt.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = getAim() * 55;
        }

        Destroy(fireBolt, 5f);
    }

    public void Attack()
    {
        player.attack.Execute(gameObject);
        isAttacking = false;
    }


  

   


   

   
}