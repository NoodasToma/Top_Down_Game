
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.UIElements;
using System;
public class SkillManager : MonoBehaviour
{
    [SerializeField] public Passive passive;

    [SerializeField] public MinorSkill minorSkill;

    [SerializeField] public Ultimate ultimate;

    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        handlePassive();
        handleMinorSkill();
        handleUltimate();
    }

    void handlePassive()
    {
        if (!passive.IsOnCd()) passive.execute(gameObject);
    }

    void handleMinorSkill()
    {
        if (minorSkill is MinorSkill minorS) {
            if (!minorSkill.IsOnCd() && Input.GetKeyDown(KeyCode.E))
            {
                minorSkill.execute(gameObject);
                startAnim(minorS.animationValue);
            }
        }
            else if (minorSkill is MinorSkillWithIndicator minorSkillWithIndicator)
            {
                if (!minorSkill.IsOnCd())
                {
                    if (Input.GetKeyDown(KeyCode.E)) minorSkillWithIndicator.OnPress();
                    if (Input.GetKey(KeyCode.E)) minorSkillWithIndicator.OnHold();
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        minorSkillWithIndicator.OnRelease(gameObject);
                        startAnim(minorSkillWithIndicator.animationValue);
                    }
                }
            }
    }

    

    void handleUltimate()
    {
        if (Input.GetKeyDown(KeyCode.R) && !ultimate.IsOnCd())
        {
            ultimate.execute(gameObject);
            startAnim(ultimate is Ultimate ultimateSkill ? ultimateSkill.animationValue : "");
        }
    }


    void startAnim(String triggerVal) {
        animator.SetTrigger(triggerVal);
    }
   
}
