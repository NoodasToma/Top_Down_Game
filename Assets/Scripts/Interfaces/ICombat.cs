using UnityEngine;
namespace Combat
{
    public interface IDamageable
    {
        void TakeDamage(Damage damage);
        void Heal(float ammount) { }
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

        public Damage(float amount, DamageType type, GameObject source, Vector3 direction, float knockBackForce)
        {
            this.amount = amount;
            this.type = type;
            this.source = source;
            this.direction = direction;
            this.knockBackForce = knockBackForce;
        }
        public Damage(float amount)
        {
            this.amount = amount;
            this.source = null;
            this.direction = Vector3.zero;
            this.type = DamageType.True;
            this.knockBackForce = 0f;

        }
        public Damage(float amount, float knockBackForce)
        {
            this.amount = amount;
            this.source = null;
            this.type = DamageType.True;
            this.direction = Vector3.zero;
            this.knockBackForce = knockBackForce;
        }
    }
    public delegate void AttackAction(GameObject attacker, Damage damage);

}