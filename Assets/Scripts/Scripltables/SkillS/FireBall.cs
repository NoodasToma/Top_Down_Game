using System.Collections;
using System.Collections.Generic;
using Combat;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

[CreateAssetMenu(menuName = "Combat/Skills/Fireball")]
public class FireBall : SkillSO
{
    [SerializeField]
    private float range;
    [SerializeField]
    private float fireballSpeed;
    


    public GameObject arrowIndicatorPrefab;
    private GameObject currentIndicator;
    public GameObject fireballPrefab;
    
    public GameObject explosionEffect;
    private LineRenderer lineRenderer;

    public override void OnRelease(GameObject caster, Vector3 aim, Damage damage)
    {
        if (!state || onCooldown)
        {
            state = false;
            Destroy(currentIndicator);
            return;
        }
        state = false;
        Debug.Log("explossion");
        Destroy(currentIndicator);
        cooldownLeft = cooldown;


        //actual fireball logic
        Vector3 spawnPos = caster.transform.position + Vector3.up * 1.6f + aim.normalized * 0.8f;

        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = aim.normalized * fireballSpeed;
        }
        Destroy(fireball, 5f);
    }


    public override void renderIndicator(Vector3 origin, Vector3 aim, bool render)
    {
        origin = origin + Vector3.up * 1f; 

        if (!render)
        {
            if (currentIndicator != null)
            {
                Debug.Log("Destroy");
                Destroy(currentIndicator); // Destroy the arrow indicator object
                currentIndicator = null;
            }
            return;
        }
        if (currentIndicator == null)
        {
            currentIndicator = Instantiate(arrowIndicatorPrefab);
            lineRenderer = currentIndicator.GetComponent<LineRenderer>();
        }
        Vector3 end = origin + Vector3.Normalize(aim) * 10f; // 5 units long line

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, end);
    }

}
