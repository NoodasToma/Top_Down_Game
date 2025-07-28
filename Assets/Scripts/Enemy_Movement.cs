using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float hp;

    private bool inRange;
    public float attackRange;
    public float movementSpeed;
    public float attackSpeed;

    public GameObject target;

    public float rotSpeed;

    public float pushability;

    public bool isKnockable;

    private Coroutine flashCoroutine;

    private Color originalColor;

    public float distanceFromAllies;

    public LayerMask layer;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(calcDistance());

        originalColor = GetComponent<Renderer>().material.color;
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

        

        if (rotLock != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, rotSpeed*Time.deltaTime);

        targetLoc = targetLoc.normalized;


        Collider[] enemiesNear = Physics.OverlapSphere(transform.position, distanceFromAllies,layer); //detect enemy colliders within distance


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

        //TODO()
    }

    // everything that needs to happen when enemy gets hit
    public void takeDamage(float damage)
    {
        // take damage
        float highlightTime = 0.25f;
        hp -= damage;
        if (hp <= 0) GameObject.Destroy(this.gameObject);



        //get knocked back
        if (isKnockable) knockBack();

        //highlight
        if (flashCoroutine != null) StopCoroutine(flashCoroutine); // if the coroutine is already running and we hit enemy again it should stop and re run
        flashCoroutine = StartCoroutine(highglightAttack(highlightTime));

    }

    IEnumerator highglightAttack(float duration) //coroutine that highlights the enemy hit for the duration and changes it back
    {
        Renderer ren = GetComponent<Renderer>();
        ren.material.color = Color.white;

        yield return new WaitForSeconds(duration);

        ren.material.color = originalColor;
    }

    void knockBack()
    {
        Vector3 direction = transform.position - target.transform.position;
        direction.y = 0;

        transform.Translate(direction * pushability * Time.deltaTime, Space.World);
    }



    // gizmos for debugging
    void OnDrawGizmosSelected()
    {
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(transform.position, distanceFromAllies); 
    }

}
