using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Movement : MonoBehaviour
{
    public float hp;

    private bool inRange;
    public float attackRange;
    public float movementSpeed;

    public GameObject target;

    public float rotSpeed;

    public float pushability;

    public bool isKnockable;

    private Coroutine flashCoroutine;

    private Color originalColor;

    public float distanceFromAllies;

    public float distanceFromWalls;

    public LayerMask enemies;

    public LayerMask walls;

    private Enemy_Attack attackScript;

    Slider enemyHealthBar;



    // Start is called before the first frame update
    void Start()
    {
        enemyHealthBar = gameObject.GetComponent<Slider>();
        enemyHealthBar.maxValue = hp;
        setHealthBar(hp);
        target = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(calcDistance());

        originalColor = GetComponent<Renderer>().material.color;

        attackScript = GetComponent<Enemy_Attack>();
    }

    IEnumerator calcDistance() // coroutine calculates distance to a player every 0.25 seconds
    {

        while (true)
        {

            float distance = (target.transform.position - transform.position).magnitude;
            inRange = distance <= attackRange;



            yield return new WaitForSeconds(0.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
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
                Vector3 awayFromWall = (awayDir.normalized / distanc) * 2.0f;

                Vector3 sidewaysFromWall = Quaternion.Euler(0, 90, 0) * awayFromWall;

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


    }





    void attack()
    {
        // rotating the target while its in range to attack the player
        Vector3 targetLoc = target.transform.position - transform.position;
        targetLoc.y = 0;
        Quaternion rotDir = Quaternion.LookRotation(targetLoc);
        if (targetLoc != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, rotSpeed);

        attackScript.Attack();
    }

    // everything that needs to happen when enemy gets hit
    public void takeDamage(float damage, float force)
    {
        // take damage
        float highlightTime = 0.25f;
        hp -= damage;
        if (hp <= 0) GameObject.Destroy(this.gameObject);



        //get knocked back
        if (isKnockable) knockBack(force);

        //highlight
        if (flashCoroutine != null) StopCoroutine(flashCoroutine); // if the coroutine is already running and we hit enemy again it should stop and re run
        flashCoroutine = StartCoroutine(highglightAttack(highlightTime));

        setHealthBar(hp);

    }

    IEnumerator highglightAttack(float duration) //coroutine that highlights the enemy hit for the duration and changes it back
    {
        Renderer ren = GetComponent<Renderer>();
        ren.material.color = Color.white;

        yield return new WaitForSeconds(duration);

        ren.material.color = originalColor;
    }

    void knockBack(float force)
    {
        Vector3 direction = transform.position - target.transform.position;
        direction.y = 0;

        transform.Translate(direction * (force / pushability) * Time.deltaTime, Space.World);
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
