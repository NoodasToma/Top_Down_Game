using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack_Script : MonoBehaviour
{
    public playerClass Class;
    private playerClassStats player = new playerClassStats();

    public LayerMask layer;

    private Animator playerAnimator;

    private Coroutine attackRoutine;
    private Coroutine skillRoutine;

    public float skillCdMinor = 5f;
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
    public float comboResetTime = 1f;
    private float lastClickTime = 0f;
    private bool isAttacking = false;




    PlayerSkillScript playerSkill;
    ThrowingItems throwItem;
    public itemClass itemClass;

    // Start is called before the first frame update
    void Start()
    {
        constructChar(Class);
        playerAnimator = GetComponent<Animator>();
        playerSkill = GetComponent<PlayerSkillScript>();
        throwItem = GetComponent<ThrowingItems>();

        //Todo  at the start assign damaga range etc based on class
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && !comboOnCd)
        {
            float lastAttackTime = Time.time - lastClickTime;
            if (lastAttackTime > comboResetTime) comboIndex = 0;
            lastClickTime = Time.time;
            if(attackRoutine==null)attackRoutine = StartCoroutine(swing());
         
         }   
        // Original full check
        if (Input.GetKeyDown(KeyCode.E) && skillRoutine == null)
        {
            skillRoutine = StartCoroutine(minorSkill());
        }
        if (Input.GetKeyDown(KeyCode.G) && throwRoutine == null)
        {
            throwRoutine = StartCoroutine(throwing());
        }
    

        checkedForchangeFortesting();

    }
    IEnumerator minorSkill()
    {
        playerSkill.minorSkill(player.playerClass);
        playerAnimator.SetTrigger("Fireball");
        yield return new WaitForSeconds(player.skillCdMinor);
        skillRoutine = null;
    }

    IEnumerator throwing()
    {
        throwItem.itemToThrow(itemClass);
        // playerAnimator.SetTrigger("rames gaaketeb");
        yield return new WaitForSeconds(throwingItemCD);
        throwRoutine = null;
    }

    IEnumerator swing()  // coroutine that manages attack cooldowns
    {
        isAttacking = true;

        float speedTemp = GetComponent<Player_Movement>().speed;
        if(!player.isRanged)GetComponent<Player_Movement>().speed = 0;
        

        switch (comboIndex)
        {
            case 0: playerAnimator.SetTrigger("Attack"); break;
            case 1: playerAnimator.SetTrigger("Fireball"); break;
            case 2: playerAnimator.SetTrigger("Dodge"); break;
        }


        Vector3 push = getAim() * 0.5f;
        transform.position += push;


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
    
    // draws a sphere around the player and checks if an enemy inside is within angle to get hit if it is it takes damage
    // void attack()
    // {
    //     Vector3 hitBoxOrigin = transform.position + getAim() * 0.5f;

    //     if (!isRanged)
    //     {

    //         Collider[] hitEnemies = Physics.OverlapSphere(hitBoxOrigin, range, layer);
    //         if (hitEnemies.Length <= 0) return;

    //         foreach (Collider c in hitEnemies)
    //         {
    //             Vector3 positionEnemy = c.transform.position - transform.position;
    //             positionEnemy.y = 0;
    //             positionEnemy = positionEnemy.normalized;
    //             float angle = Vector3.Angle(getAim(), positionEnemy);
    //             if (angle <= angleOfAttack && c.gameObject != null)
    //             {
    //                 c.gameObject.GetComponent<Enemy_Movement>().takeDamage(damage, forceOfAttack);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         RaycastHit hit;
    //         bool isHit = Physics.SphereCast(hitBoxOrigin, radiusOfRangedAttack, getAim(), out hit, range, layer);
    //         Debug.Log(isHit);
    //         if (isHit)
    //         {
    //             Debug.Log(hit.transform.name);
    //             hit.transform.gameObject.GetComponent<Enemy_Movement>().takeDamage(damage, forceOfAttack);
    //         }

    //     }

    // }





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

    void basicAttack(playerClass playerClass)
    {
        Vector3 hitBoxOrigin = transform.position + getAim() * 0.5f;
        switch (playerClass)
        {
            case playerClass.Sorcerer:
                RangerAttack(hitBoxOrigin);
                break;
            case playerClass.Fighter:
                FighterAttack(hitBoxOrigin);
                break;
            case playerClass.Rogue:
                //Todo
                break;
            case playerClass.Ranger:
                RangerAttack(hitBoxOrigin);
                break;
            case playerClass.Alchemist:
                //Todo
                break;
            case playerClass.Warlock:
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

        foreach (Collider c in hitEnemies)
        {
            Vector3 positionEnemy = c.transform.position - transform.position;
            positionEnemy.y = 0;
            positionEnemy = positionEnemy.normalized;
            float angle = Vector3.Angle(getAim(), positionEnemy);
            if (angle <= player.angleOfAttack && c.gameObject != null)
            {
                c.gameObject.GetComponent<Enemy_Movement>().takeDamage(player.damage, player.forceOfAttack);
            }
        }
    }

    void RangerAttack(Vector3 originOfattack)
    {
        RaycastHit hit;
        bool isHit = Physics.SphereCast(originOfattack, player.radiusOfRangedAttack, getAim(), out hit, player.range, layer);
        Debug.Log(isHit);
        if (isHit)
        {
            Debug.Log(hit.transform.name);
            hit.transform.gameObject.GetComponent<Enemy_Movement>().takeDamage(player.damage, player.forceOfAttack);
        }
    }


    void constructChar(playerClass playerClass)
    {
        switch (playerClass)
        {
            case playerClass.Sorcerer:
                player = new playerClassStats(Class, test_damage, test_range, test_isRanged,
                 test_cooldownOfAttack, test_angleOfAttack, test_radiusOfRangedAttack, test_forceOfAttack, test_skillCdMinor);
                break;
            case playerClass.Fighter:
                player = new playerClassStats(Class, test_damage, test_range, test_isRanged,
                test_cooldownOfAttack, test_angleOfAttack, test_radiusOfRangedAttack, test_forceOfAttack, test_skillCdMinor);
                break;
            case playerClass.Rogue:
                player = new playerClassStats(Class, test_damage, test_range, test_isRanged,
                test_cooldownOfAttack, test_angleOfAttack, test_radiusOfRangedAttack, test_forceOfAttack, test_skillCdMinor);
                break;
            case playerClass.Ranger:
                player = new playerClassStats(Class, test_damage, test_range, test_isRanged,
                test_cooldownOfAttack, test_angleOfAttack, test_radiusOfRangedAttack, test_forceOfAttack, test_skillCdMinor);
                break;
            case playerClass.Alchemist:
                player = new playerClassStats(Class, test_damage, test_range, test_isRanged,
                test_cooldownOfAttack, test_angleOfAttack, test_radiusOfRangedAttack, test_forceOfAttack, test_skillCdMinor);
                break;
            case playerClass.Warlock:
                player = new playerClassStats(Class, test_damage, test_range, test_isRanged,
                test_cooldownOfAttack, test_angleOfAttack, test_radiusOfRangedAttack, test_forceOfAttack, test_skillCdMinor);
                break;
            default:

                break;
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

        if (player.forceOfAttack != test_forceOfAttack)
            player.SetForceOfAttack(test_forceOfAttack);

        if (player.skillCdMinor != test_skillCdMinor)
            player.SetSkillCdMinor(test_skillCdMinor);
        if(player.playerClass!=Class) player.SetPlayerClass(Class);
    }
    
}
