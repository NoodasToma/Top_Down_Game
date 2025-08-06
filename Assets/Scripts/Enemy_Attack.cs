using System.Collections;
using UnityEngine;
using Combat;
public class Enemy_Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage;
    public float attackCooldown;
    public float attackRange;
    public float attackHurtboxRadius;

    [Header("Debug")]
    public bool showGizmos = true;

    private float _nextAttackTime;
    public Player_Movement playerScript;

    private Animator enemyAttackAnimator;

    private Coroutine attackRoutine;

    public float enemyAttackForce; //determines how far  player goes when hit

    public float enemyAttackStagger; // determines how long player gets staggered


    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player_Movement>();
        enemyAttackAnimator = gameObject.GetComponent<Animator>();
   

    void Start()
    {
    }

    void Update()
    {

    }




    public void Attack()
    {

        Collider[] hitColliders = Physics.OverlapSphere(
        transform.position + transform.forward * attackHurtboxRadius * 0.5f,
        attackHurtboxRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                var player = col.GetComponent<Player_Movement>();
                if (playerScript.alive)
                {
                    player.TakeDamage(attackDamage, enemyAttackForce, enemyAttackStagger, gameObject);
                    var player = col.GetComponent<IDamageable>();
                    player.TakeDamage(new Damage(attackDamage));
                    break;
                }
            }
        }


    }


    IEnumerator AttackRoutineEnemy()
    {



        enemyAttackAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);

        attackRoutine = null;

    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = new Color(1, 0, 1, 0.5f);
        Gizmos.DrawSphere(
            transform.position + transform.forward * attackHurtboxRadius * 0.5f,
            attackHurtboxRadius
        );
    }


    public void AttackAnimationTrigger()
    {
        if(attackRoutine==null) attackRoutine = StartCoroutine(AttackRoutineEnemy());
    }
}