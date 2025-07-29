using UnityEngine;

public class FireballTrigger : MonoBehaviour
{
    public float damage = 50f;
    public float knockbackForce = 10f;
    public float explosionRadius = 3f;
    public GameObject explosionEffect;
    public LayerMask enemyLayer;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Fireball hit: " + other.name); // Add this line

        // Only explode if hitting something on enemy layer
        Explode();
        

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

           c.gameObject.GetComponent<Enemy_Movement>().takeDamage(damage, knockbackForce);
            
        }

        Destroy(gameObject);
    }
}