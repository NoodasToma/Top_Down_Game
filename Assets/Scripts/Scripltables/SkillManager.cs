using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
public class SkillManager : MonoBehaviour
{
    public SkillSO minorSkill;
    public SkillSO ulty;
    // fireball cooldown. if you change this make sure to change cooldown times in the UI script too    
    private PlayerAttack_Script playerAttack_Script;
    private Animator playerAnimator;
    void Start()
    {
        minorSkill = Instantiate(GetComponent<StatsManager>().classs.minorSkill);
        minorSkill.hideFlags = HideFlags.DontSave;

        playerAnimator = GetComponent<Animator>();
        playerAttack_Script = GetComponent<PlayerAttack_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        minorSkill.updatCooldown(Time.deltaTime);
        if (minorSkill.state) { UpdateAiming();};


        if (Input.GetMouseButtonDown(1)) // Right-click cancels skill aiming
        {
            CancelSkill(minorSkill); // Call cancel method
        }

    }

    public void Skill1(InputAction.CallbackContext context)
    {
        Damage damage = new Damage();        
        Vector3 dir = playerAttack_Script.getAim();
        switch (context.phase)
        {
            case InputActionPhase.Started:
                UpdateAiming();
                minorSkill.OnStart(this.gameObject, dir, damage); break;
            case InputActionPhase.Canceled:
                playerAnimator.SetTrigger("Fireball");
                minorSkill.OnRelease(this.gameObject, dir, damage);
                minorSkill.onCooldown = true;
                break;
            default:  break;
        }
    }

    void UpdateAiming()
    {
        Vector3 origin = transform.position + Vector3.up * 1f; // adjust height
        Vector3 dir = playerAttack_Script.getAim();
        minorSkill.renderIndicator(origin, dir, true);
    }
    void CancelSkill(SkillSO skill)
    {
        skill.state = false;
    }
   
}
