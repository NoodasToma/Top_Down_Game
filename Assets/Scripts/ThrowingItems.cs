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
    private GameObject indicatorInstance;
    public float maxThrowRange = 15f;
    public LayerMask groundLayer;

    private bool isAiming = false;
    private itemClass currentItemToThrow;
    private Vector3 throwTarget;

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

    // Public method to start aiming
    public void StartAiming(itemClass item)
    {
        isAiming = true;
        currentItemToThrow = item;
    }

    // Public method to execute the throw
    public void ExecuteThrow()
    {
        if (!isAiming || indicatorInstance == null) return;
        
        ThrowItemAtIndicator(currentItemToThrow);
        isAiming = false;
        
        if (indicatorInstance != null)
            Destroy(indicatorInstance);
    }

    

    // Public method to cancel aiming
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
            
            // Calculate horizontal distance only
            float horizontalDistance = Vector3.Distance(
                new Vector3(playerPos.x, 0, playerPos.z),
                new Vector3(targetPoint.x, 0, targetPoint.z)
            );
            
            if (horizontalDistance > maxThrowRange)
            {
                Vector3 direction = (targetPoint - playerPos).normalized;
                targetPoint = playerPos + direction * maxThrowRange;
                targetPoint.y = hit.point.y; // Maintain original Y position
            }

            if (indicatorInstance == null)
                indicatorInstance = Instantiate(indicatorPrefab, targetPoint, Quaternion.identity);

            indicatorInstance.transform.position = targetPoint;
            throwTarget = targetPoint; // Store the target for throwing
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