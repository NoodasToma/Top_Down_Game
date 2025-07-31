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


    // Start is called before the first frame update
    void Start()
    {
        playerAttack_Script = GetComponent<PlayerAttack_Script>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void fireball()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 1.6f + playerAttack_Script.getAim() * 0.8f;

        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = playerAttack_Script.getAim() * fireballSpeed;
        }

        Destroy(fireball, 5f);


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
