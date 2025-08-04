using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

// [CreateAssetMenu(menuName = "Registry/Skill")]s
public abstract class SkillSO : ScriptableObject
{
    public Damage damage;
    public float cooldown;
    public bool onCooldown;

    public virtual void OnStart(GameObject caster, Vector3 aim, Damage damage) { }
    public virtual void OnHold(GameObject caster, Vector3 aim, Damage damage){}
    public virtual void OnRelease(GameObject caster, Vector3 aim, Damage damage){}

    public virtual void Execute(GameObject caster, Vector3 aim, Damage damage) { }
    public virtual void Execute(GameObject caster, Vector3 aim) {
    }
    public abstract IEnumerator Use(Vector3 origin, Vector3 aim);
    public virtual void renderIndicator(Vector3 origin, Vector3 aim, bool render) { }
}
