using System;
using Combat;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class Enemy_Movement : MonoBehaviour, IDamageable
{

    public float hp;

    private bool inRange;
    private float attackRange;
    public float movementSpeed;

    public GameObject target;

    public float rotSpeed;

    public float weight;

    public bool isKnockable;

    private Coroutine flashCoroutine;

    private Color originalColor;

    public float distanceFromAllies;

    public float distanceFromWalls;

    public LayerMask enemies;

    public LayerMask walls;

    private Enemy_Attack attackScript;

    Slider enemyHealthBar;
    public GameObject bloodSplatterPrefab;

    private Animator animationController;

    public enum ENEMY_STATE
    {
        Basic,Parried, Staggered

    }

    public ENEMY_STATE enemyState = ENEMY_STATE.Basic;





    // Start is called before the first frame update
    void Start()
    {
        enemyHealthBar = gameObject.GetComponent<Slider>();
        enemyHealthBar.maxValue = hp;
        setHealthBar(hp);
        target = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(calcDistance());

        originalColor = GetComponentInChildren<Renderer>().material.color;

        attackScript = GetComponent<Enemy_Attack>();

        animationController = GetComponent<Animator>();

        attackRange = attackScript.attackRange;
    }

    IEnumerator calcDistance() // coroutine calculates distance to a player every 0.25 seconds
    {

        while (true)
        {
            if (gameObject != null)
            {
                float distance = (target.transform.position - transform.position).magnitude;
                inRange = distance <= attackRange;



                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(enemyState.ToString());
        if (target == null) return; 
        if (enemyState == ENEMY_STATE.Staggered) return;
        if (!inRange) moveTowardsPlayer();
        else attack();

    }



    void moveTowardsPlayer() // Moves enemy towards player changing rotation and avoiding other enemies
    {
        Vector3 targetLoc = target.transform.position - transform.position;

        Vector3 rotLock = new Vector3(targetLoc.x, 0, targetLoc.z);

        Quaternion rotDir = Quaternion.LookRotation(rotLock);
        

        if (rotLock != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, rotSpeed * Time.deltaTime);

        targetLoc = targetLoc.normalized;


        Collider[] enemiesNear = Physics.OverlapSphere(transform.position, distanceFromAllies, enemies); //detect enemy colliders within distance
        Collider[] wallsNear = Physics.OverlapSphere(transform.position, distanceFromWalls, walls);


        //same thing for walls
        if (wallsNear.Length > 0)
        {
            foreach (Collider c in wallsNear)
            {

                if (c.gameObject == gameObject) continue;
                GameObject wallCollided = c.gameObject;

                Vector3 awayDir = transform.position - c.ClosestPoint(transform.position);

                float distanc = awayDir.magnitude;
                Vector3 awayFromWall = (awayDir.normalized / distanc) * 5.0f;
                
                Vector3 sidewaysFromWall = Quaternion.Euler(0, UnityEngine.Random.Range(90, 180), 0) * awayFromWall ;

              
                
                if (distanc > 0.1f) targetLoc += awayFromWall + sidewaysFromWall;


            }
        }

        // check if enemies are colliding with each other and change direction accordingly

        if (enemiesNear.Length > 0)
        {
            foreach (Collider c in enemiesNear)
            {
                if (c.gameObject == gameObject) continue;
                GameObject enemyCollided = c.gameObject;

                Vector3 awayDir = transform.position - enemyCollided.transform.position;

                float distanc = awayDir.magnitude;
                if (distanc > 0.1f) targetLoc += awayDir.normalized / distanc;


            }
        }


        targetLoc.y = 0;

        Vector3 direction = new Vector3(targetLoc.normalized.x, 0f, targetLoc.normalized.z);
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        if(!animationController.GetCurrentAnimatorStateInfo(0).IsName("GotHit"))animationController.SetBool("Walking", true);


    }





    void attack()
    {
        // rotating the target while its in range to attack the player
        Vector3 targetLoc = target.transform.position - transform.position;
        targetLoc.y = 0;
        Quaternion rotDir = Quaternion.LookRotation(targetLoc);
        if (targetLoc != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, rotSpeed);

        attackScript.AttackAnimationTrigger();
    }

    // everything that needs to happen when enemy gets hit
    public void TakeDamage(float damage, float force,float stagger)
    {
        TakeDamage(new Damage(damage, force,stagger));
    }
    public void TakeDamage(Damage damage)
    {
        // take damage
        float highlightTime = damage.staggerDuration;


        if (enemyState == ENEMY_STATE.Parried) hp -= damage.amount*2;
        else hp -= damage.amount;

      

        //get knocked back
        if (isKnockable) knockBack(damage.knockBackForce);

        //highlight
        if (flashCoroutine == null) flashCoroutine = StartCoroutine(highglightAttack(highlightTime)); // if the coroutine is already running and we hit enemy again it should stop and re run
        Debug.Log(highlightTime);


        setHealthBar(hp);
    }
    public void Heal(float amount)
    {
        hp += amount;
        setHealthBar(hp);
    }
    IEnumerator highglightAttack(float duration)
    {
        Renderer ren = GetComponentInChildren<Renderer>();
        ren.material.color = Color.white;  // Highlight enemy red on hit

        // Instantiate blood splatter effect prefab at enemy's position
        GameObject bloodSplatter = Instantiate(bloodSplatterPrefab, transform.position, Quaternion.identity);

        // Optional: Parent the splatter to the enemy so it moves with them
        bloodSplatter.transform.SetParent(transform);
        animationController.SetBool("Walking", false);
        animationController.SetTrigger("GotHIt");



        enemyState = ENEMY_STATE.Staggered;
        // Wait for the duration of the highlight
      
        yield return new WaitForSeconds(duration);

        animationController.SetBool("Walking", true);
        // Revert color
        ren.material.color = originalColor;

        // Destroy blood splatter effect after highlight ends
        Destroy(bloodSplatter);

        flashCoroutine = null;

        if (hp <= 0)
        {
            GameEventManager.EnemyKilled();

            Destroy(gameObject.GetComponent<CapsuleCollider>());

            animationController.SetBool("Walking", false);

            animationController.SetTrigger("Death");
            movementSpeed = 0;
            rotSpeed = 0;

            yield return new WaitForSeconds(2f);

            Destroy(gameObject);
        


        }
        flashCoroutine = null;
        enemyState = ENEMY_STATE.Basic;
  } 

    void knockBack(float force)
    {
        Vector3 direction = transform.position - target.transform.position;
        direction.y = 0;

        transform.Translate(direction * (force / weight) * Time.deltaTime, Space.World);
    }



    // gizmos for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceFromAllies);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanceFromWalls);
    }


    void setHealthBar(float val)
    {
        enemyHealthBar.value = val;
    }


   

}
