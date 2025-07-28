using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Script : MonoBehaviour
{
    public float damage;

    public float range;

    public float cooldown;
    public float angleOfAttack;

    public LayerMask layer;

    private Animator playerAnimator;

    private Coroutine attackRoutine;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)&&attackRoutine==null) attackRoutine = StartCoroutine(swing());

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
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range,layer);
        if (hitEnemies.Length <= 0) return;
    
        foreach (Collider c in hitEnemies)
        {
            Vector3 positionEnemy = c.transform.position - transform.position;
            positionEnemy.y = 0;
            positionEnemy = positionEnemy.normalized;
            float angle = Vector3.Angle(getAim(), positionEnemy);
            if (angle < angleOfAttack && c.gameObject != null)
            {
                c.gameObject.GetComponent<Enemy_Movement>().takeDamage(damage);
            }
        }

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range); // Match radius

        Vector3 center = getAim();
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, center * range);

        Vector3 left = Quaternion.Euler(0, angleOfAttack, 0) * center;
        Vector3 right = Quaternion.Euler(0, -angleOfAttack, 0) * center;

        Gizmos.DrawRay(transform.position, left * range);
        Gizmos.DrawRay(transform.position, right * range);

        
    }
}
