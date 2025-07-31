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


    // public interface IProjectile
    // {
    //     public GameObject owner {get ; set;}
    //     public ProjectileEffect onHitEffect{get; set;}
    //     public void OnHit(GameObject target)
    //     {
    //         onHitEffect?.Invoke(this);
    //     }
    // }