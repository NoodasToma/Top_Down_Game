using UnityEngine;
namespace Combat
{
    public delegate void ProjectileEffect(IProjectile p);
    
    public interface IProjectile
    {
        Damage damage{ get; set; }
        public void OnHit(GameObject target)
        {
        }
    }
}
