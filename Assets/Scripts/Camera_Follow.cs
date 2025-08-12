using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{

    private Transform target;
    public float smoothTime;
    public Vector3 baseOffset;

    public Vector3 maxOffset = new Vector3(10,0,10);

    private Vector3 velocity = Vector3.zero;

    public Vector3 currentOffset;
    public float deadZoneRadius;

    public bool cameraLock =false;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;  // find player
        currentOffset = baseOffset;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) cameraLock = !cameraLock;
        if (cameraLock)
        {
            currentOffset = baseOffset;
            return;
        }
        Vector3 mouseDir = GetMouseDirectionWithDeadZone();

        // Calculate target offset scaled by maxOffset limits
        Vector3 targetOffset =baseOffset +  new Vector3(
            mouseDir.x * maxOffset.x,
            baseOffset.y,
            mouseDir.y * maxOffset.z
        );


        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * 5f);
        currentOffset.x = Mathf.Clamp(currentOffset.x, baseOffset.x - maxOffset.x, baseOffset.x + maxOffset.x);
        currentOffset.y = baseOffset.y;  // keep fixed if you want
        currentOffset.z = Mathf.Clamp(currentOffset.z, baseOffset.z - maxOffset.z, baseOffset.z + maxOffset.z);



    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + currentOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); // Move camera to player overtime
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

    }


    Vector3 GetMouseDirectionWithDeadZone()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        Vector3 dir = mousePos - screenCenter;
        float distance = dir.magnitude;

        if (distance < deadZoneRadius)
            return Vector3.zero;  // inside dead zone, no movement

        return dir.normalized;  // outside dead zone, normalized direction
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(target.transform.position,currentOffset);
    }
}
