using System;
using UnityEngine;
namespace Combat
{
    public interface IDamageable
    {
        void TakeDamage(Damage damage);
        void Heal(float ammount) { }

        void SetHealth(float health) { }
    }
    public interface IAttack
    {
        void execute(Damage damage, Vector3 direction);
    }
    public interface IKillable
    {
        void death(GameObject killer);
    }
    public enum DamageType
    {
        Physical,
        Fire,
        Ice,
        Poison,
        True
    }

    public struct Damage
    {
        public float amount;
        public DamageType type;
        public GameObject source;
        public Vector3 direction;
        public float knockBackForce;

        public float staggerDuration;


        public Damage(float amount, DamageType type, GameObject source, Vector3 direction, float knockBackForce, float staggerDuration)
        {
            this.amount = amount;
            this.type = type;
            this.source = source;
            this.direction = direction;
            this.knockBackForce = knockBackForce;
            this.staggerDuration = staggerDuration;
        }
        
        public Damage(float amount)
        {
            this.amount = amount;
            this.source = null;
            this.direction = Vector3.zero;
            this.type = DamageType.True;
            this.knockBackForce = 0f;
            this.staggerDuration = 0f;

        }
        public Damage(float amount, float knockBackForce)
        {
            this.amount = amount;
            this.source = null;
            this.type = DamageType.True;
            this.direction = Vector3.zero;
            this.knockBackForce = knockBackForce;
            this.staggerDuration = 0f;
        }

        public Damage(float amount, float knockBackForce, float staggerDuration)
        {
            this.amount = amount;
            this.source = null;
            this.type = DamageType.True;
            this.direction = Vector3.zero;
            this.knockBackForce = knockBackForce;
            this.staggerDuration = staggerDuration;
        }
        
         public Damage(float amount, float knockBackForce,float staggerDuration,GameObject source)
        {
            this.amount = amount;
            this.source = source;
            this.type = DamageType.True;
            this.direction = Vector3.zero;
            this.knockBackForce = knockBackForce;
            this.staggerDuration = staggerDuration;
        }
    }
    public delegate void AttackAction(GameObject attacker, Damage damage);

}