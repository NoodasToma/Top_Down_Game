using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Skills/RangerSkill")]
public class RangerSkill : MinorSkillWithIndicator
{
    public GameObject spikes;

    public override void OnHold()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPress()
    {
        throw new System.NotImplementedException();
    }

   

    public override void skill(GameObject caster)
    {
         Debug.Log("SKill used" + getSpawnpos());
        Instantiate(spikes,getSpawnpos(),Quaternion.identity);
    }

    private Vector3 getSpawnpos()
    {
        Vector3 spawnPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().rotationTarget;
        spawnPos.y = 0.01f;
        return spawnPos;
    }

}
