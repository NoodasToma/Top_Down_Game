using UnityEngine;
using Combat;
public class ProjectileCollision : MonoBehaviour
{
    public float damage = 100f;
    public float knockbackForce = 10f;
    public float explosionRadius = 3f;
    public GameObject explosionEffect;
    public LayerMask enemyLayer;
    public float healAmount = 50f;
    public float healRadius = 3f;
    public GameObject healEffect;
    public bool isHealing = false;


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Projectile hit: " + other.name);

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

            c.gameObject.GetComponent<IDamageable>().TakeDamage(new Damage(-healAmount)); 

        }

        Destroy(gameObject);
    }
    
}