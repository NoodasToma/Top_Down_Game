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

    private bool alive = true;

    Ui_script ui_Script;

    private enum state
    {
        Basic, Dodging

    }

    private state currentState;
    public float dashSpeed;





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
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mouseLook);

            if (Physics.Raycast(ray, out hit)) rotationTarget = hit.point;

            movePlayerWithAim();
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentState == state.Dodging) return;
        playerHP -= damage;

        // Visual feedback (flash effect)
        // Visual feedback (flash effect)
        StartCoroutine(DamageFlash());

        if (playerHP <= 0)
        {
            playerHP = 0;
            alive = false;
            GameObject.Destroy(gameObject.GetComponent<PlayerAttack_Script>());
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
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
        }
        Vector3 movementDir = new Vector3(move.x, 0f, move.y);
        
        if (currentState != state.Dodging && Input.GetKeyDown(KeyCode.LeftShift))
        {
            dodge(movementDir);
        }
        else
        {


            movingAnim(movementDir);


            transform.Translate(movementDir * speed * Time.deltaTime, Space.World);

            playerAnimator.SetBool("MovesBack", isMovingBackwards(movementDir, lookPos));

        }
        

        // Debug.Log("Movement Dir is" + " " + Vector3.Normalize(vector3) + " " + "Look dir is " + " " + Vector3.Normalize(lookPos));


    }

    public Vector3 getDirection() // direction player is looking at needed for player_attack_script
    {
        Vector3 lookPos = rotationTarget - transform.position;
        lookPos.y = 0;
        return lookPos.normalized;
    }

    bool isMovingBackwards(Vector3 v1, Vector3 v2) // check if enemy is moving and facing in opposite directions
    {
        v1 = Vector3.Normalize(v1);
        v2 = Vector3.Normalize(v2);

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
    
        dodge(dodgeDir);
        yield return new WaitForSeconds(0.25f);
        currentState = state.Basic;
    }

    void dodge(Vector3 dodgeDir)
    {
        float tempSpeed = dashSpeed;
        currentState = state.Dodging;
        while (tempSpeed > 0)
        {
            transform.Translate(dodgeDir * tempSpeed * Time.deltaTime, Space.World);
            tempSpeed-=tempSpeed/10*Time.deltaTime;
        }
    }
}
