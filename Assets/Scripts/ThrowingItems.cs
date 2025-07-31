using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ThrowingItems : MonoBehaviour
{
    public GameObject throwablePrefab;
    public GameObject healingPrefab; 
    public float throwableSpeed;
    private PlayerAttack_Script playerAttack_Script;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerAttack_Script = GetComponent<PlayerAttack_Script>();

    }

    //capital T is neccessary unity already has some bullshit throw function
    void throwItem()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 1.4f + playerAttack_Script.getAim() * 0.8f;

        GameObject throwableItem = Instantiate(throwablePrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = throwableItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = playerAttack_Script.getAim() * throwableSpeed;
        }

        Destroy(throwableItem, 5f);


    }
    
    public void itemToThrow(itemClass itemClass)
    {
        switch (itemClass)
        {
            case itemClass.HealingPotion:
            throwablePrefab = healingPrefab;
                throwItem();
                break;
            case itemClass.Grenade:
                //Todo
             break;
            
           
        }
    }
}
