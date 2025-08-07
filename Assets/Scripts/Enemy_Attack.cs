using UnityEngine;
using Combat;
using System.Collections;
public class Enemy_Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage;
    public float attackCooldown;
    public float attackRange;
    public float attackHurtboxRadius;
    public float enemyAttackForce; //determines how far  player goes when hit
    public float enemyAttackStagger; // determines how long player gets staggered
    [Header("Debug")]
    public bool showGizmos = true;

    private float _nextAttackTime;

    private Animator enemyAttackAnimator;

    private Coroutine attackRoutine;

    



    void Start()
    {
         enemyAttackAnimator = GetComponent<Animator>();
    }

    void Update()
    {
      
    }

    public void Attack()
    {
         if (Time.time >= _nextAttackTime)
        {
             Collider[] hitColliders = Physics.OverlapSphere(
             transform.position + transform.forward * attackHurtboxRadius * 0.5f,
             attackHurtboxRadius
            );

            foreach (Collider col in hitColliders)
            {
                if (col.CompareTag("Player"))
                {
                    var player = col.GetComponent<IDamageable>();
                    player.TakeDamage(new Damage(attackDamage,DamageType.Physical,gameObject,gameObject.transform.position-col.gameObject.transform.position,enemyAttackForce,enemyAttackStagger));
                    break;
                }
            }
            _nextAttackTime = Time.time + attackCooldown;
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