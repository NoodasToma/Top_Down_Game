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
    public float playerHP = 100f;

    private Vector2 move, mouseLook;

    public float rotationSpeed;

    private Vector3 rotationTarget;

    public Animator playerAnimator;
    private Material _originalMaterial;

    public bool alive = true;

    Ui_script ui_Script;

    private enum state
    {
        Basic, Dodging

    }

    private state currentState;
    public float dashSpeed;


    public float dodgeDuration = 0.25f;

    public float iFrameDuration;
    public float dodgeCooldown = 1f;

    private bool isDodging = false;
    private bool dodgeOnCooldown = false;

    [SerializeField]
    private Rigidbody rb;
    public LayerMask walls;


     


    // Start is called before the first frame update
    void Start()
    {
        currentState = state.Basic;
        ui_Script = GameObject.FindGameObjectWithTag("HpBar").GetComponent<Ui_script>();
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            _originalMaterial = new Material(renderer.material); // Create a copy
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (alive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging && !dodgeOnCooldown)
            {
                Vector3 dodgeDir = new Vector3(move.x, 0f, move.y).normalized;
                if (dodgeDir != Vector3.zero)
                {
                    StartCoroutine(dodgeRoutine(dodgeDir));
                }
            }
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mouseLook);

            if (Physics.Raycast(ray, out hit)) rotationTarget = hit.point;
        }
    }
    void FixedUpdate()
    {
        if (alive)
        {
            movePlayerWithAim();

            debugIframes();
        }
    }
    void FixedUpdate()
{
    if (!alive) return;
    movePlayerWithAim(); 
}


    public void TakeDamage(float damage)
    {
        if (!alive) return;
        if (currentState == state.Dodging) return;

        Character_Passives passives = GetComponent<Character_Passives>();
        PlayerAttack_Script attackScript = GetComponent<PlayerAttack_Script>();

        if (passives != null && attackScript != null && attackScript.Class == playerClass.Sorcerer)
        {
            // Let Character_Passives handle damage and cheat death for Sorcerer
            passives.CheckForCheatDeath(damage);
            return; // Exit so damage is handled only in CheckForCheatDeath
        }


        playerHP -= damage;

        // Visual feedback (flash effect)
        // Visual feedback (flash effect)
        StartCoroutine(DamageFlash());

        if (playerHP <= 0 && alive)
        {
            playerHP = 0;
            alive = false;
            ui_Script.gameOver();
            GameObject.Destroy(gameObject.GetComponent<PlayerAttack_Script>());
            playerAnimator.SetTrigger("Dead");
        }

        ui_Script.setHpBar(playerHP);
    }

    IEnumerator DamageFlash()
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer != null && _originalMaterial != null)
        {
            // Create a temporary material instance for flashing
            renderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            // Restore the original material properties
            renderer.material.CopyPropertiesFromMaterial(_originalMaterial);
        }
    }


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


    // move player while changing the aim direction simalteniously

    void movePlayerWithAim()
    {

        Vector3 lookPos = rotationTarget - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);

        Vector3 aimDir = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

        if (aimDir != Vector3.zero)
        {
            // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, rotationSpeed  * Time.fixedDeltaTime));

        }
        Vector3 movementDir = new Vector3(move.x, 0f, move.y);


        if (movingToWall(movementDir)) movementDir = Vector3.zero;

        movingAnim(movementDir);


        // transform.Translate(movementDir * speed * Time.deltaTime, Space.World);  no mor transform
        rb.velocity = movementDir * speed;

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
        currentState = state.Dodging;

        float fallingDashspeed = dashSpeed;

        float timer = 0f;

        playerAnimator.SetTrigger("Dodge");

        while (timer < dodgeDuration)
        {
            // transform.Translate(dodgeDir * fallingDashspeed * Time.deltaTime, Space.World);
            rb.velocity = dodgeDir * fallingDashspeed;
            timer += Time.deltaTime;
            fallingDashspeed -= dashSpeed / 10 * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(iFrameDuration);
        currentState = state.Basic;
        isDodging = false;

        yield return new WaitForSeconds(dodgeCooldown); // cooldown wait
        dodgeOnCooldown = false;

    }

    void debugIframes()
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (currentState == state.Dodging) renderer.material.color = Color.black;
        else renderer.material.color = Color.white;
    }

    bool movingToWall(Vector3 playerMovingDir)
    {
        RaycastHit hit;
        bool hitsWall = Physics.SphereCast(transform.position,1f, playerMovingDir.normalized, out hit, 1f, walls);
        return hitsWall;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 movementDir = new Vector3(move.x, 0f, move.y);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, movementDir.normalized*2f);
    }


}
