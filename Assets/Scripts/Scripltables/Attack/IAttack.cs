using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using System;
namespace Combat
{
    [CreateAssetMenu(menuName = "Combat//Attack")]
    public abstract class Attack : ScriptableObject
    {
        [Header("Attack Settings")]
        [SerializeField] protected Damage damageValue;

        [SerializeField] protected float rangeValue;
        [SerializeField] protected String animationValue;
        [SerializeField] protected LayerMask layerMaskValue;

        // Optional public getters if other scripts need access
        public Damage DamageValue => damageValue;

        public float RangeValue => rangeValue;
        public String AnimationValue => animationValue;
        public LayerMask LayerMaskValue => layerMaskValue;





        public abstract void Execute(GameObject attacker, GameObject target);
        public abstract void Execute(GameObject attacker);
    }


  
}