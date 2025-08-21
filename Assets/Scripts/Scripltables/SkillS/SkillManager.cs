
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.UIElements;
public class SkillManager : MonoBehaviour
{
    public SkillSO minorSkill;
    public SkillSO ulty;

    public SkillSO passive;
    private PlayerAttack_Script playerAttack_Script; // redundant
    private Animator playerAnimator;

    private Ui_script ui_Script;

    private bool lastIsUlty; //temp fix
    void Start()
    {
        ui_Script = GetComponent<Ui_script>();
        if (ui_Script == null)
            ui_Script = GameObject.FindGameObjectWithTag("HpBar")?.GetComponent<Ui_script>();


        minorSkill = Instantiate(GetComponent<StatsManager>().classs.minorSkill);
        if (minorSkill != null)
        {
            minorSkill.hideFlags = HideFlags.DontSave;
            ui_Script?.SetSkillIcon(minorSkill?.skillIcon);
        }

        ulty = Instantiate(GetComponent<StatsManager>().classs.minorSkill);
        if (minorSkill != null) ulty.hideFlags = HideFlags.DontSave;

        playerAnimator = GetComponent<Animator>();
        playerAttack_Script = GetComponent<PlayerAttack_Script>();
        passive = GetComponent<StatsManager>().classs.passive != null ? Instantiate(GetComponent<StatsManager>().classs.passive) : null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = PlayerAttack_Script.getAim();

        minorSkill.updatCooldown(Time.deltaTime);
        if (minorSkill.state) { UpdateAiming(); minorSkill.OnHold(gameObject, dir, new Damage()); }
        ;

        ulty?.updatCooldown(Time.deltaTime);
        if (ulty != null && ulty.state)
        {
            UpdateAiming();
            ulty.OnHold(gameObject, dir, new Damage());
        }

        if (Input.GetMouseButtonDown(1)) // Right-click cancels skill aiming
        {
            CancelSkill(minorSkill); // Call cancel method
            CancelSkill(ulty);
        }

        if (passive != null) passive.Passive();

    }

    public void Skill1(InputAction.CallbackContext context)
    {
        Vector3 dir = PlayerAttack_Script.getAim();
        Damage damage = new Damage();        
        switch (context.phase)
        {
            case InputActionPhase.Started:
                minorSkill.OnStart(this.gameObject, dir, damage); break;
            case InputActionPhase.Canceled:
                playerAnimator.SetTrigger("Fireball");//idk how exactly animations work needs update
                minorSkill.OnRelease(this.gameObject, dir, damage);
                minorSkill.onCooldown = true;
                ui_Script.SkillCD(minorSkill.cooldown);
                break;
            default:  break;
        }
    }
    public void Ulty(InputAction.CallbackContext context)
    {
        Damage damage = new Damage();        
        Vector3 dir = PlayerAttack_Script.getAim();
        switch (context.phase)
        {
            case InputActionPhase.Started:
                ulty.OnStart(this.gameObject, dir, damage); break;
            case InputActionPhase.Canceled:
                //animation
                ulty.OnRelease(this.gameObject, dir, damage);
                ulty.onCooldown = true;
                break;
            default:  break;
        }
    }
    void UpdateAiming()
    {
        Vector3 origin = gameObject.transform.position;
        Vector3 aim = PlayerAttack_Script.getAim();
        minorSkill.renderIndicator(origin, aim, !lastIsUlty);// if ulty was pressed last dont render minor skill else render 
        ulty.renderIndicator(origin, aim, lastIsUlty); //if ulty was pressed last render ullyt else dont
    }
    void CancelSkill(SkillSO skill)
    {
        if (skill != null) skill.state = false;
    }
   
}
