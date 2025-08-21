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
        public Vector3 directionValue { get; protected set; }

        public Animation animationValue { get; protected set; }

        public LayerMask layerMaskValue { get; protected set; }

        public Attack(Damage damage, Vector3 direction)
        {
            damageValue = damage;
            directionValue = direction;
        }
        public Attack(Damage damage, Vector3 direction, Animation animation, LayerMask layerMask)
        {
            damageValue = damage;
            directionValue = direction;
            animationValue = animation;
            layerMaskValue = layerMask;
        }
        public Attack(Damage damage, Vector3 direction, LayerMask layerMask)
        {
            damageValue = damage;
            directionValue = direction;
            layerMaskValue = layerMask;
        }

        public abstract void Execute(GameObject attacker, GameObject target);
    }
}