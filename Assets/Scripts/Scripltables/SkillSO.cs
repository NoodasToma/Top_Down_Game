using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;


// [CreateAssetMenu(menuName = "Registry/Skill")]s
public abstract class SkillSO : ScriptableObject
{
    public Damage damage;
    public float cooldown;
    public float cooldownLeft;
    public bool onCooldown;

    public bool state;

    public virtual void OnStart(GameObject caster) { state = true; }

    public virtual void OnStart(GameObject caster, Vector3 aim, Damage damage) { state = true; }
    public virtual void OnHold(GameObject caster, Vector3 aim, Damage damage) { }
    public virtual void OnRelease(GameObject caster, Vector3 aim, Damage damage) { }

    public virtual void renderIndicator(Vector3 origin, Vector3 aim, bool render) { }

    public virtual void setCooldownLeft(float amount)
    {
        if (amount <= 0f) onCooldown = false;
        else cooldownLeft = amount;
        return;
    }
    public virtual void updatCooldown(float delta)
    {
        if (onCooldown)
            setCooldownLeft(cooldownLeft - delta);
    }

    // public virtual Skill getInstance(){ return new Skill(this); }

    public virtual GameObject getIndicator()
    {
        return null;
    }
    public Sprite skillIcon;

}
