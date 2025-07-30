using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack_Script : MonoBehaviour
{
    public float damage;

    public float range;

    public bool isRanged;

    public float cooldown;
    public float angleOfAttack;

    public float radiusOfRangedAttack;

    public float forceOfAttack;

    public LayerMask layer;

    private Animator playerAnimator;

    private Coroutine attackRoutine;
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public float fireballExplosionRadius = 3f;
    public GameObject explosionEffect;
    // fireball cooldown. if you change this make sure to change cooldown times in the UI script too 
    public float fireballCDTime = 5f;
    private Coroutine fireballRoutine;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackRoutine == null) attackRoutine = StartCoroutine(swing());
        // Original full check
        if (Input.GetKeyDown(KeyCode.E) && fireballRoutine == null)
        {
            fireballRoutine = StartCoroutine(fire());
        }
    
    }
    IEnumerator fire() {
        fireball();
        playerAnimator.SetTrigger("Fireball");
         yield return new WaitForSeconds(fireballCDTime);
        fireballRoutine = null;
    }

    IEnumerator swing()  // coroutine that manages attack cooldowns
    {
        playerAnimator.SetTrigger("Attack");

        
        yield return new WaitForSeconds(cooldown);

        attack();

        attackRoutine = null;
    }

    // draws a sphere around the player and checks if an enemy inside is within angle to get hit if it is it takes damage
    void attack()
    {  
       Vector3 hitBoxOrigin = transform.position + getAim() * 0.5f;

        if (!isRanged)
        {

            Collider[] hitEnemies = Physics.OverlapSphere(hitBoxOrigin, range, layer);
            if (hitEnemies.Length <= 0) return;

            foreach (Collider c in hitEnemies)
            {
                Vector3 positionEnemy = c.transform.position - transform.position;
                positionEnemy.y = 0;
                positionEnemy = positionEnemy.normalized;
                float angle = Vector3.Angle(getAim(), positionEnemy);
                if (angle <= angleOfAttack && c.gameObject != null)
                {
                    c.gameObject.GetComponent<Enemy_Movement>().takeDamage(damage, forceOfAttack);
                }
            }
        }
        else
        {
            RaycastHit hit;
            bool isHit = Physics.SphereCast(hitBoxOrigin, radiusOfRangedAttack, getAim(), out hit, range, layer);
            Debug.Log(isHit);
            if (isHit)
            {
                Debug.Log(hit.transform.name);
                hit.transform.gameObject.GetComponent<Enemy_Movement>().takeDamage(damage, forceOfAttack);
            }
            
        }

    }
    void fireball()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 1.6f + getAim() * 0.8f;

        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = getAim() * fireballSpeed;
        }

        Destroy(fireball, 5f);
        

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
        if (!isRanged)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range); // Match radius

            Vector3 center = getAim();
            Gizmos.color = Color.green;
            Gizmos.DrawRay(hitBoxOrigin, center * range);

            Vector3 left = Quaternion.Euler(0, angleOfAttack, 0) * center;
            Vector3 right = Quaternion.Euler(0, -angleOfAttack, 0) * center;

            Gizmos.DrawRay(hitBoxOrigin, left * range);
            Gizmos.DrawRay(hitBoxOrigin, right * range);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hitBoxOrigin, getAim() * range);
        }
        

        
    }
}
