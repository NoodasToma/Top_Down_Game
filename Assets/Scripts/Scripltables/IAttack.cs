using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public abstract class Attack
{
    public Damage damageValue { get; protected set; }
    public Vector3 direction { get; protected set; }

    public Attack(Damage damage, Vector3 direction)
    {
        damageValue = damage;
        direction = direction;
    }

    public abstract void Execute(GameObject attacker, GameObject target);
}