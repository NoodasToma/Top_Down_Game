using UnityEngine;
using Combat;
public class ProjectileCollision : MonoBehaviour
{
    public float damage;
    public float knockbackForce;
    public float explosionRadius;
    public GameObject explosionEffect;
    public LayerMask enemyLayer;
    public float healAmount;
    public float healRadius;
    public GameObject healEffect;
    public bool isHealing = false;


    void OnTriggerEnter(Collider other)
    {
        

        if (isHealing)
        {
            Heal(); // <-- Added: call Heal if it's a healing item
        }
        else
        {
            Explode(); // <-- Otherwise explode as usual
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider c in hitEnemies)
        {
            Vector3 knockDir = (c.transform.position - transform.position).normalized;
            knockDir.y = 0;

            c.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(damage, knockbackForce));

        }

        Destroy(gameObject);
    }
    
    void Heal()
    {
        if (healEffect != null)
            Instantiate(healEffect, transform.position, Quaternion.identity);

       Collider[] hitEnemies = Physics.OverlapSphere(transform.position, healRadius, enemyLayer);
        foreach (Collider c in hitEnemies)
        {
            Vector3 knockDir = (c.transform.position - transform.position).normalized;
            knockDir.y = 0;

            c.gameObject.GetComponent<IDamageable>().Heal(healAmount); 

        }

        Destroy(gameObject);
    }
    
}