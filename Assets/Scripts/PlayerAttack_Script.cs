using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Combat;

public class PlayerAttack_Script : MonoBehaviour
{
    public PlayerClass pClass; //temporal for quick class switch

    public ClassSO player;
    public LayerMask layer;

    public LayerMask wall;

    private Animator playerAnimator;

    private Coroutine attackRoutine;
    private Coroutine skillRoutine;

    private Coroutine frameFreezer;

    [SerializeField]
    private GameObject fireBoltPrefab;

    public float throwingItemCD = 5f;
    private Coroutine throwRoutine;

    public float test_damage;

    public float test_range;

    public bool test_isRanged;

    public float test_cooldownOfAttack;
    public float test_angleOfAttack;

    public float test_radiusOfRangedAttack;

    public float test_forceOfAttack;

    public float test_skillCdMinor;

    public float comboCd;

    private bool comboOnCd;

    private int comboIndex = 0;
    public float comboResetTime;
    private float lastClickTime = 0f;
    private bool isAttacking = false;
    private StatsManager statsManager;


    ThrowingItems throwItem;
    public itemClass itemClass;

    private bool isAimingThrow = false;

    // Start is called before the first frame update
    void Start()
    {
        constructChar(player.playerClass);
        playerAnimator = GetComponent<Animator>();
        throwItem = GetComponent<ThrowingItems>();
        statsManager = GetComponent<StatsManager>();
    
        //Todo  at the start assign damaga range etc based on class
    }

    // Update is called once per frame
    void Update()
    {
        if (statsManager.currentState == StatsManager.STATE.Staggered) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && !comboOnCd)
        {
            float lastAttackTime = Time.time - lastClickTime;
            if (lastAttackTime > comboResetTime) comboIndex = 0;
            lastClickTime = Time.time;
            if (attackRoutine == null) attackRoutine = StartCoroutine(swing());
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
        checkedForchangeFortesting();

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


    IEnumerator swing()  // coroutine that manages attack cooldowns
    {
        isAttacking = true;

        float speedTemp = GetComponent<Player_Movement>().speed;
        if (!player.isRanged) GetComponent<Player_Movement>().speed = speedTemp / 4;


        switch (comboIndex)
        {
            case 0: playerAnimator.SetTrigger("Attack"); break;
            case 1: playerAnimator.SetTrigger("Fireball"); break;
            case 2: playerAnimator.SetTrigger("Attack"); break;
        }


        // Vector3 push = getAim() * 1f;
        // transform.position += push;


        yield return new WaitForSeconds(player.cooldownOfAttack);

        basicAttack(player.playerClass);

        comboIndex++;

        if (comboIndex > 2)
        {
            comboIndex = 0;
            comboOnCd = true;
            yield return new WaitForSeconds(comboCd);  // Cooldown before new combo
            comboOnCd = false;
        }

        isAttacking = false;

        GetComponent<Player_Movement>().speed = speedTemp;


        attackRoutine = null;
    }

    //returns the direction player is lookin
    public Vector3 getAim()
    {
        Vector3 v = this.gameObject.GetComponent<Player_Movement>().getDirection();
        v.y = 0;
        return v.normalized;
    }

    //gizmos for debuggin
    void OnDrawGizmosSelected()
    {
        Vector3 hitBoxOrigin = transform.position + getAim() * 0.5f;
        if (!player.isRanged)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, player.range); // Match radius

            Vector3 center = getAim();
            Gizmos.color = Color.green;
            Gizmos.DrawRay(hitBoxOrigin, center * player.range);

            Vector3 left = Quaternion.Euler(0, player.angleOfAttack, 0) * center;
            Vector3 right = Quaternion.Euler(0, -player.angleOfAttack, 0) * center;

            Gizmos.DrawRay(hitBoxOrigin, left * player.range);
            Gizmos.DrawRay(hitBoxOrigin, right * player.range);
        }
        else

        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hitBoxOrigin, getAim() * player.range);
        }



    }

    void basicAttack(PlayerClass playerClass)
    {
        Vector3 hitBoxOrigin = transform.position + getAim() * 0.5f;
        switch (playerClass)
        {
            case PlayerClass.Sorcerer:
                sorcererAttack();
                break;
            case PlayerClass.Fighter:
                FighterAttack(hitBoxOrigin);
                break;
            case PlayerClass.Rogue:
                //Todo
                break;
            case PlayerClass.Ranger:
                RangerAttack(hitBoxOrigin);
                break;
            case PlayerClass.Alchemist:
                //Todo
                break;
            case PlayerClass.Warlock:
                //Todo
                break;
            default:

                break;
        }
    }

    void FighterAttack(Vector3 originOfattack)
    {

        Collider[] hitEnemies = Physics.OverlapSphere(originOfattack, player.range, layer);
        if (hitEnemies.Length <= 0) return;
        CameraShake(0.1f, 0.2f);
        freezeFrame(0.05f);
        foreach (Collider c in hitEnemies)
        {
            Vector3 positionEnemy = c.transform.position - transform.position;
            positionEnemy.y = 0;
            positionEnemy = positionEnemy.normalized;
            float angle = Vector3.Angle(getAim(), positionEnemy);
            if (angle <= player.angleOfAttack && c.gameObject != null)
            {
                float finalDamage = player.damage * (statsManager != null ? statsManager.damageMultiplier : 1f);
                c.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(finalDamage, player.kncokback));
            }
        }
    }

    void RangerAttack(Vector3 originOfattack)
    {
        RaycastHit hit;
        RaycastHit wallhit;
        bool isHit = Physics.SphereCast(originOfattack, player.radiusOfRangedAttack, getAim(), out hit, player.range,layer);
        bool isWall = Physics.SphereCast(originOfattack, player.radiusOfRangedAttack, getAim(), out wallhit, player.range,wall);

      
    
        if (isHit || isWall)
        {
            
            if (isHit && isWall)
            {
             Debug.Log(originOfattack);
             Debug.Log(hit.collider.name);
             Debug.Log(wallhit.collider.name);
             if ((wallhit.transform.position - originOfattack).magnitude <= (hit.transform.position - originOfattack).magnitude) return;
            }

            if (!isHit) return;
            CameraShake(0.05f, 0.1f);
            freezeFrame(0.05f);
            Debug.Log(hit.transform.name);
            float finalDamage = player.damage * (statsManager != null ? statsManager.damageMultiplier : 1f);
            hit.transform.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(finalDamage, player.kncokback));
        }
    }
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


    void constructChar(PlayerClass playerClass)
    {
        switch (playerClass)
        {
        }
    }

    void checkedForchangeFortesting()
    {
        if (player == null) return;

        if (player.damage != test_damage)
            player.SetDamage(test_damage);

        if (player.range != test_range)
            player.SetRange(test_range);

        if (player.isRanged != test_isRanged)
            player.SetIsRanged(test_isRanged);

        if (player.cooldownOfAttack != test_cooldownOfAttack)
            player.SetCooldownOfAttack(test_cooldownOfAttack);

        if (player.angleOfAttack != test_angleOfAttack)
            player.SetAngleOfAttack(test_angleOfAttack);

        if (player.radiusOfRangedAttack != test_radiusOfRangedAttack)
            player.SetRadiusOfRangedAttack(test_radiusOfRangedAttack);

        if (player.kncokback != test_forceOfAttack)
            player.SetForceOfAttack(test_forceOfAttack);
        if (player.playerClass != pClass) player.SetPlayerClass(pClass);
    }


    void CameraShake(float duration, float magnitude)
    {
        GameObject cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder");
        Vector3 orignalPos = cameraHolder.transform.localPosition;
        StartCoroutine(screenShaker(duration, magnitude, cameraHolder, orignalPos));
    }

    IEnumerator screenShaker(float duration, float magnitude, GameObject cameraholder, Vector3 originalPos)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {

            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            cameraholder.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            timePassed += Time.deltaTime;

            yield return null;
        }

        cameraholder.transform.localPosition = originalPos;
    }

    void freezeFrame(float duration)
    {
        if(frameFreezer==null) frameFreezer = StartCoroutine(frameFreeze(duration));
    }

    IEnumerator frameFreeze(float duration)
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
        frameFreezer = null;
    }
}