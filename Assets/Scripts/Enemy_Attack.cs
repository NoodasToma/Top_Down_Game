using UnityEngine;

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
   

    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player_Movement>();
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
                    var player = col.GetComponent<Player_Movement>();
                    if (playerScript.alive)
                    {
                        player.TakeDamage(attackDamage);
                        break;
                    }
                }
            }
            _nextAttackTime = Time.time + attackCooldown;
        }
        
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
}