using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingItems : MonoBehaviour
{
    public GameObject throwablePrefab;
    public GameObject healingPrefab;
    public float throwableSpeed;
    private PlayerAttack_Script playerAttack_Script;

    public GameObject indicatorPrefab;
    public GameObject indicatorInstance;
    public float maxThrowRange = 15f;
    public LayerMask groundLayer;

    private bool isAiming = false;
    private itemClass currentItemToThrow;
    private Vector3 throwTarget;

    public LayerMask obstacleLayer; 
     
    

    void Start()
    {
        playerAttack_Script = GetComponent<PlayerAttack_Script>();
    }

    void Update()
    {
        // Only handle aiming when requested
        if (isAiming)
        {
            UpdateThrowIndicator();
        }
    }
    public void StartAiming(itemClass item)
    {
        isAiming = true;
        currentItemToThrow = item;
    }
    public void ExecuteThrow()
    {
        if (!isAiming || indicatorInstance == null) return;
        
        ThrowItemAtIndicator(currentItemToThrow);
        isAiming = false;
        
        if (indicatorInstance != null)
            Destroy(indicatorInstance);
    }

    public void CancelAiming()
    {
        isAiming = false;
        if (indicatorInstance != null)
            Destroy(indicatorInstance);
    }

    void UpdateThrowIndicator()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            Vector3 playerPos = transform.position;

            Vector3 toTarget = targetPoint - playerPos;
            toTarget.y = 0f; // Flatten to XZ plane
            if (toTarget.magnitude > maxThrowRange)
            {
                toTarget = toTarget.normalized * maxThrowRange;
                targetPoint = playerPos + toTarget;
            }

            Vector3 playerEyePos = transform.position + Vector3.up * 1.5f;
            Vector3 dir = (targetPoint - playerEyePos).normalized;
            float dist = Vector3.Distance(playerEyePos, targetPoint);

            RaycastHit obstacleHit;
            Vector3 finalPoint = targetPoint;

            if (Physics.Raycast(playerEyePos, dir, out obstacleHit, dist, obstacleLayer))
            {
                finalPoint = obstacleHit.point;
            }

            finalPoint.y += 0.05f; // offset to prevent clipping into ground
            if (indicatorInstance == null)
            {
                indicatorInstance = Instantiate(indicatorPrefab);
            }
            indicatorInstance.transform.position = finalPoint;

        indicatorInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            indicatorInstance.SetActive(true);
            throwTarget = finalPoint;
    }
        else
        {
            if (indicatorInstance != null)
            {
                indicatorInstance.SetActive(false);
            }
        }
}

    public void ThrowItemAtIndicator(itemClass itemClass)
    {
        Vector3 start = transform.position + Vector3.up * 1.1f;

        GameObject prefabToThrow = GetPrefabForItem(itemClass);
        if (prefabToThrow == null) return;

        GameObject thrownItem = Instantiate(prefabToThrow, start, Quaternion.identity);
        Collider playerCollider = GetComponent<Collider>();
    Collider itemCollider = thrownItem.GetComponent<Collider>();
    if (playerCollider != null && itemCollider != null)
    {
        Physics.IgnoreCollision(playerCollider, itemCollider);
    }
        Rigidbody rb = thrownItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 velocity = CalculateArcVelocity(start, throwTarget, 1f);
            rb.velocity = velocity;
        }

        Destroy(thrownItem, 5f);
    }

    Vector3 CalculateArcVelocity(Vector3 start, Vector3 end, float time)
    {
        Vector3 distance = end - start;
        Vector3 horizontal = new Vector3(distance.x, 0f, distance.z);
        float vertical = distance.y;

        float horizontalDistance = horizontal.magnitude;
        float horizontalSpeed = horizontalDistance / time;
        float verticalSpeed = vertical / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 velocity = horizontal.normalized * horizontalSpeed;
        velocity.y = verticalSpeed;
        return velocity;
    }

    GameObject GetPrefabForItem(itemClass item)
    {
        switch (item)
        {
            case itemClass.HealingPotion:
                return healingPrefab;
            case itemClass.Grenade:
                return throwablePrefab;
            default:
                return null;
        }
    }
}