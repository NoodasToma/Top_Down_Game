using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        // Make the canvas face the camera, but lock rotation so it doesn’t tilt
        Vector3 lookDir = transform.position - mainCam.transform.position;
        lookDir.x = lookDir.z = 0f; // Keep only vertical axis
        transform.rotation = Quaternion.LookRotation(mainCam.transform.forward, Vector3.up);
    }
}
