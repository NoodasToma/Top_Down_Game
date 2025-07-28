using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 10f;
    public float attackCooldown = 10f;
    public float attackRange = 1.5f;
    public float attackHurtboxRadius = 1f;

    [Header("Debug")]
    public bool showGizmos = true;

    private float _nextAttackTime;
    private Enemy_Movement _movement;

    void Start()
    {
        _movement = GetComponent<Enemy_Movement>();
    }

    void Update()
    {
        if (_movement == null || !_movement.InAttackRange()) return;

        if (Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void Attack()
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
                if (player != null)
                {
                    player.TakeDamage(attackDamage);
                    break;
                }
            }
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