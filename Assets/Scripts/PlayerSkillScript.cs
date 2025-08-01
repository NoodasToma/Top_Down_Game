using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillScript : MonoBehaviour
{   
    
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public float fireballExplosionRadius = 3f;
    public GameObject explosionEffect;
    // fireball cooldown. if you change this make sure to change cooldown times in the UI script too    
    private PlayerAttack_Script playerAttack_Script;

    public GameObject arrowIndicatorPrefab;
    private GameObject currentIndicator;
    private LineRenderer lineRenderer;

    private bool isAiming = false;


    // Start is called before the first frame update
    void Start()
    {
        playerAttack_Script = GetComponent<PlayerAttack_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && isAiming)
        UpdateAiming();
    }

    void fireball()
    {
        GameObject.Destroy(currentIndicator);
        Vector3 spawnPos = transform.position + Vector3.up * 1.6f + playerAttack_Script.getAim() * 0.8f;

        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = playerAttack_Script.getAim() * fireballSpeed;
        }

        Destroy(fireball, 5f);


    }

    void UpdateAiming()
    {
    Vector3 origin = transform.position + Vector3.up * 1f; // adjust height
    Vector3 dir = playerAttack_Script.getAim();
    Vector3 end = origin + dir * 10f; // 5 units long line

    lineRenderer.SetPosition(0, origin);
    lineRenderer.SetPosition(1, end);

    // Optional: rotate an arrow sprite or mesh instead
    }

    void startAim()
    {
        isAiming = true;
        currentIndicator = Instantiate(arrowIndicatorPrefab);
        lineRenderer = currentIndicator.GetComponent<LineRenderer>();
    }
    public void CancelAiming()
{
    isAiming = false;
    
    if (currentIndicator != null)
    {
        Destroy(currentIndicator); // Destroy the arrow indicator object
        currentIndicator = null;
    }
}

    public void AimSkill(playerClass playerClass)
    {
        switch (playerClass)
        {
            case playerClass.Sorcerer:
                startAim();
                break;
            case playerClass.Fighter:
                //Todo
                break;
            case playerClass.Rogue:
                //Todo
                break;
            case playerClass.Ranger:
                //Todo
                break;
            case playerClass.Alchemist:
                //Todo
                break;
            case playerClass.Warlock:
                //Todo
                break;
            default:

                break;
        }
    }

    public void minorSkill(playerClass playerClass)
    {
        switch (playerClass)
        {
            case playerClass.Sorcerer:
                fireball();
                break;
            case playerClass.Fighter:
                //Todo
                break;
            case playerClass.Rogue:
                //Todo
                break;
            case playerClass.Ranger:
                //Todo
                break;
            case playerClass.Alchemist:
                //Todo
                break;
            case playerClass.Warlock:
                //Todo
                break;
            default:

                break;
        }
    }


    


   
}
