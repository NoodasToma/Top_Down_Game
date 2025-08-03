using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public class SkillManager : MonoBehaviour
{
    public SkillSO skill;
    // fireball cooldown. if you change this make sure to change cooldown times in the UI script too    
    private PlayerAttack_Script playerAttack_Script;
    private Animator playerAnimator;
    private Coroutine skillRoutine;
    private bool skillOnCD;
    private bool isAiming = false;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerAttack_Script = GetComponent<PlayerAttack_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 origin = transform.position + Vector3.up * 1f; // adjust height
        Vector3 dir = playerAttack_Script.getAim();
        Damage damage = new Damage();
        if (Input.GetKeyDown(KeyCode.E) && !skillOnCD) { skill.OnStart(this.gameObject, dir, damage); isAiming = true; }
        if (Input.GetKey(KeyCode.E) && !skillOnCD) {UpdateAiming(); skill.OnHold(this.gameObject, dir, damage); }
        if (Input.GetKeyUp(KeyCode.E) && isAiming && skillRoutine == null)
        {
            skillOnCD = true;
            skillRoutine = StartCoroutine(minorSkill()); // Actually fire the skill
            isAiming = false; // Reset aiming state
        }
        if (isAiming && Input.GetMouseButtonDown(1)) // Right-click cancels skill aiming
        {
            CancelSkill(); // Call cancel method
        }

    }

    void UpdateAiming()
    {
        Vector3 origin = transform.position + Vector3.up * 1f; // adjust height
        Vector3 dir = playerAttack_Script.getAim();
        skill.renderIndicator(origin, dir, true);
    }


    public void CancelAiming()
    {
        skill.renderIndicator(Vector3.zero, Vector3.zero, false);
    }

    IEnumerator minorSkill()
    {
        Vector3 dir = playerAttack_Script.getAim();
        skill.OnRelease(this.gameObject, dir, new Damage());
        playerAnimator.SetTrigger("Fireball");
        yield return new WaitForSeconds(skill.cooldown);
        skillRoutine = null;
        skillOnCD = false;
    }

    void CancelSkill()
    {
        isAiming = false;
        CancelAiming(); // Tells skill script to stop aiming and hide indicator
    }


    public void Logger(string message)
    {
        Debug.Log("Skillamangager:" + message);
    }
   
}
