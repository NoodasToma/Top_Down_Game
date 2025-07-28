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






    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mouseLook);

        if (Physics.Raycast(ray, out hit)) rotationTarget = hit.point;

        movePlayerWithAim();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();

    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();

    }


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

        Vector3 vector3 = new Vector3(move.x, 0f, move.y);

        movingAnim(vector3);


        transform.Translate(vector3 * speed * Time.deltaTime, Space.World);

        playerAnimator.SetBool("MovesBack", isMovingBackwards(vector3, lookPos));


        // Debug.Log("Movement Dir is" + " " + Vector3.Normalize(vector3) + " " + "Look dir is " + " " + Vector3.Normalize(lookPos));

        
    }

    public Vector3 getDirection()
    {
        Vector3 lookPos = rotationTarget - transform.position;
        lookPos.y = 0;
        return lookPos.normalized;
    }

    bool isMovingBackwards(Vector3 v1, Vector3 v2)
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
}
