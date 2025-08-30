using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using System;



public interface SkillSO
{
    public bool IsOnCd();
    public void execute(GameObject caster);
}

public abstract class Passive : ScriptableObject, SkillSO
{
    public virtual void execute(GameObject caster)
    {
        throw new NotImplementedException();
    }

    public virtual bool IsOnCd()
    {
        return false;
    }
}

public abstract class CdPassive : Passive
{
    public float cd;



    public GameObject screenIcon;

    public bool onCd;

    public override bool IsOnCd()
    {
        return onCd;
    }

    public override void execute(GameObject caster)
    {
        onCd = true;
        passive(caster);
        GameEventManager.gameEventManager.StartCoroutine(coolDownRoutine());
    }
    public virtual IEnumerator coolDownRoutine()
    {
        yield return new WaitForSeconds(cd);
        onCd = false;
    }

    public abstract void passive(GameObject caster);
}


public abstract class MinorSkill : ScriptableObject, SkillSO
{
    public float cd;
    public String animationValue;

    public bool onCd;

    public GameObject screenIcon;

    public virtual IEnumerator coolDownRoutine()
    {

        yield return new WaitForSeconds(cd);
        onCd = false;
    }

    public bool IsOnCd()
    {
        return onCd;
    }



    public virtual void execute(GameObject caster)
    {
        onCd = true;
        skill(caster);
        GameEventManager.gameEventManager.StartCoroutine(coolDownRoutine());
    }

    public abstract void skill(GameObject caster);
    
    
}

public abstract class MinorSkillWithIndicator : MinorSkill
{

    GameObject indicator;
    public abstract void OnPress();

    public abstract void OnHold();

    public virtual void OnRelease(GameObject caster) {
        execute(caster);
    }
}

public abstract class Ultimate : ScriptableObject, SkillSO
{
    public float maxGauge;
    float gauge;
    public String animationValue;
    public bool gaugeFilled;

    GameObject screenIcon;

    public bool IsOnCd()
    {
        return gaugeFilled;
    }

    public void execute(GameObject caster)
    {
        ult(caster);
        gaugeFilled=false;
    }

    public abstract void ult(GameObject caster);

    public void fillGouge(float amounth)
    {
        gauge += amounth;
        if (gauge >= maxGauge) gaugeFilled = true;
    }
}

