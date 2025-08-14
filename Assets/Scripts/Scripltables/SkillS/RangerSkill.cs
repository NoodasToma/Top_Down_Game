using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Skills/RangerSkill")]
public class RangerSkill : SkillSO
{
    public GameObject spikes;


    public override void OnRelease(GameObject caster, Vector3 aim, Damage damage)
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
