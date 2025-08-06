using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
namespace Combat
{
    // [CreateAssetMenu(menuName = "Combat")]
    public abstract class Attack : ScriptableObject
    {
        public Damage damageValue { get; protected set; }
        public Vector3 direction { get; protected set; }

        public Attack(Damage damage, Vector3 direction)
        {
        }

        public abstract void Execute(GameObject attacker, GameObject target);
    }
}