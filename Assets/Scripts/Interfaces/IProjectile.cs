using UnityEngine;
namespace Combat
{
    public delegate void ProjectileEffect(IProjectile p);
    public interface IProjectile
    {
        public void OnHit(GameObject target)
        {
        }
    }
}
