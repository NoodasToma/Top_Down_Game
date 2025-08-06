using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class Player_Movement : MonoBehaviour
{

    private Camera mainCamera;
    public float speed;
    private Vector2 move, mouseLook;

    public float rotationSpeed;

    private Vector3 rotationTarget;

    public Animator playerAnimator;

    public bool alive = true;


    public enum state
    {
        Basic, Dodging, Staggered,Recovering

    }

    public state currentState;
    public StatsManager stats;
    public float dashSpeed;


    public float dodgeDuration = 0.25f;

    public float iFrameDuration;
    public float dodgeCooldown = 1f;

    public float RecoveryDuration;

    private bool isDodging = false;
    private bool dodgeOnCooldown = false;

    [SerializeField]
    private Rigidbody rb;
    public LayerMask walls;


     


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatsManager>();
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == state.Staggered) return;
       
        if (alive)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mouseLook);
            if (Physics.Raycast(ray, out hit)) rotationTarget = hit.point;
        }
    }
    void FixedUpdate()
{
    if (!alive||currentState==state.Staggered) return;
    movePlayerWithAim(); 
}


    public void TakeDamage(float damage ,float knockBackDis,float staggerDuration, GameObject source)
    {
        if (!alive) return;
        if (currentState == state.Dodging||currentState==state.Staggered) return;

        Character_Passives passives = GetComponent<Character_Passives>();
        PlayerAttack_Script attackScript = GetComponent<PlayerAttack_Script>();

        if (passives != null && attackScript != null && attackScript.Class == playerClass.Sorcerer)
        {
            // Let Character_Passives handle damage and cheat death for Sorcerer
            passives.CheckForCheatDeath(damage);
            StartCoroutine(StaggerRoutine(knockBackDis,staggerDuration,calcStaggerDir(source)));
            return; // Exit so damage is handled only in CheckForCheatDeath
        }


        playerHP -= damage;

        // Visual feedback (flash effect)
        // Visual feedback (flash effect)
        if (playerHP <= 0 && alive)
        {
            playerHP = 0;
            alive = false;
            ui_Script.gameOver();
            GameObject.Destroy(gameObject.GetComponent<PlayerAttack_Script>());
            playerAnimator.SetTrigger("Dead");
        }

        ui_Script.setHpBar(playerHP);
        
        StartCoroutine(StaggerRoutine(knockBackDis,staggerDuration,calcStaggerDir(source)));

        
        
    }

    IEnumerator StaggerRoutine(float knockBackDis,float staggerDuration, Vector3 knockBackDir)
    {

        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && _originalMaterial != null)
        {
            // Create a temporary material instance for flashing
            renderer.material.color = Color.red;
    {
        if (alive)
        {
            movePlayerWithAim();
            debugIframes();
        }

        if (currentState != state.Recovering)
        {
            currentState = state.Staggered;
            knockBackDir.Set(knockBackDir.x, 0, knockBackDir.z);
            transform.position += knockBackDir*knockBackDis;
            yield return new WaitForSeconds(staggerDuration);

            if (renderer != null && _originalMaterial != null)
            {
                renderer.material.CopyPropertiesFromMaterial(_originalMaterial);
            }
            currentState = state.Recovering;
            yield return new WaitForSeconds(RecoveryDuration);
            currentState = state.Basic;
        }

        
        
    }

    private Vector3 calcStaggerDir(GameObject source)
    {
        return ((transform.position - source.transform.position)).normalized;
    }

    


    



//input events
    //get wasd input
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();

    }
    // get mouse location
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();

    }

    public void OnDodge(InputAction.CallbackContext context)
    {
         if (alive)
        {
            if (!isDodging && !dodgeOnCooldown)
            {
                Vector3 dodgeDir = new Vector3(move.x, 0f, move.y).normalized;
                if (dodgeDir != Vector3.zero)
                {
                    StartCoroutine(dodgeRoutine(dodgeDir));
                }
            }
        }
    }
    // move player while changing the aim direction simalteniously

    void movePlayerWithAim()
    {

        Vector3 lookPos = rotationTarget - rb.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);

        Vector3 aimDir = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

        if (aimDir != Vector3.zero)
        {
            // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, rotationSpeed  * Time.fixedDeltaTime));

        }
        Vector3 movementDir = new Vector3(move.x, 0f, move.y);


        // if (movingToWall(movementDir)) movementDir = Vector3.zero;

        movingAnim(movementDir);


        // transform.Translate(movementDir * speed * Time.deltaTime, Space.World);  //no mor transform
        rb.MovePosition(rb.position + movementDir * speed * Time.fixedDeltaTime);
        // rb.velocity = movementDir * speed;

        playerAnimator.SetBool("MovesBack", isMovingBackwards(movementDir, lookPos));



        // Debug.Log("Movement Dir is" + " " + Vector3.Normalize(vector3) + " " + "Look dir is " + " " + Vector3.Normalize(lookPos));

        // Debug.Log(currentState);

    }

    public Vector3 getDirection() // direction player is looking at needed for player_attack_script
    {
        Vector3 lookPos = rotationTarget - rb.position;
        lookPos.y = 0;
        return lookPos.normalized;
    }

    bool isMovingBackwards(Vector3 v1, Vector3 v2) // check if enemy is moving and facing in opposite directions
    {
        v1 = v1.normalized;
        v2 = v2.normalized;

        if (Vector3.Dot(v1, v2) < 0) return true;

        return false;

    }

    void movingAnim(Vector3 v1) // switches animations between idle and moving
    {
        if (v1 != Vector3.zero)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }

    }

    IEnumerator dodgeRoutine(Vector3 dodgeDir)
    {

        isDodging = true;
        dodgeOnCooldown = true;
        stats.currentState = StatsManager.STATE.Dodging;

        float fallingDashspeed = dashSpeed;

        float timer = 0f;

        playerAnimator.SetTrigger("Dodge");

        while (timer < dodgeDuration)
        {
            // transform.Translate(dodgeDir * fallingDashspeed * Time.deltaTime, Space.World);
            rb.MovePosition(rb.position + dodgeDir * fallingDashspeed * Time.deltaTime);

            // rb.MovePosition(dodgeDir * fallingDashspeed * Time.deltaTime)
            // rb.velocity = dodgeDir * fallingDashspeed;
            timer += Time.deltaTime;
            fallingDashspeed -= dashSpeed / 10 * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(iFrameDuration);
        stats.currentState = StatsManager.STATE.Basic;
        isDodging = false;

        yield return new WaitForSeconds(dodgeCooldown); // cooldown wait
        dodgeOnCooldown = false;

    }

    void debugIframes()
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (stats.currentState == StatsManager.STATE.Dodging) renderer.material.color = Color.black;
        else renderer.material.color = Color.white;
    }

    // bool movingToWall(Vector3 playerMovingDir)
    // {
    //     RaycastHit hit;
    //     bool hitsWall = Physics.SphereCast(transform.position,1f, playerMovingDir.normalized, out hit, 1f, walls);
    //     return hitsWall;
    // }

    void OnDrawGizmosSelected()
    {
        Vector3 movementDir = new Vector3(move.x, 0f, move.y);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, movementDir.normalized*2f);
    }


}
