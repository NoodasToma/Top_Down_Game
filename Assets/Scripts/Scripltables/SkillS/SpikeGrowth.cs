using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using UnityEngine;

public class SpikeGrowth : MonoBehaviour
{

    public float radius;

    public float speedReduction;

    public LayerMask layerMask;

    public float duration;

    private Collider[] enemiesSlowed = new Collider[0] ;

    public float damagePerSec;

    private Coroutine damageRoutine;

    void Start()
    {
        
        StartCoroutine(InitialDelay());

        StartCoroutine(timer());
        
    }
    void Update()
    {
        Collider[] enemiesInside = Physics.OverlapSphere(transform.position, radius, layerMask);

        foreach (Collider enemy in enemiesInside)
        {
            if (enemy.gameObject != null && !enemiesSlowed.Contains(enemy) || enemiesSlowed == null && enemy.gameObject != null)
            {
                enemy.gameObject.GetComponent<Enemy_Movement>().movementSpeed /= speedReduction; 
            }
           if(damageRoutine==null) damageRoutine=StartCoroutine(damage(enemy.gameObject.GetComponent<Enemy_Movement>()));
        }

     if (enemiesSlowed != null) { 
        foreach (Collider enemy in enemiesSlowed)
        {
            if (!enemiesInside.Contains(enemy))
            {
                enemy.gameObject.GetComponent<Enemy_Movement>().movementSpeed *= speedReduction;
            }
        }
      }

        enemiesSlowed = enemiesInside;



    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    IEnumerator damage(Enemy_Movement enemy)
    {
        enemy.TakeDamage(new Damage(1, 0, 0.1f, gameObject));
        Debug.Log("Damage Delt");
        yield return new WaitForSeconds(1f);
        damageRoutine = null;
    }

     IEnumerator InitialDelay()
    {
        yield return null; // Wait for physics update
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere (transform.position, radius);
    }
}
