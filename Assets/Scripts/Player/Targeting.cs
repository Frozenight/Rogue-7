using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    private InputReader playerInput;
    private List<Transform> enemies = new List<Transform>(); // List to store enemy transforms
    private int currentTargetIndex = -1; // Index of the currently targeted enemy
    private Transform currentTarget; // Reference to the currently targeted enemy

    public float searchRadius = 10f; // Radius to search for enemies
    [SerializeField] private GameObject underTargetVFX;

    private void Awake()
    {
        playerInput = GetComponent<InputReader>();
    }

    private void Start()
    {
        playerInput.OnTargeting += TargetAndSelectNext;
    }

    private void TargetAndSelectNext()
    {
        // Clear the list of enemies
        enemies.Clear();

        // Search for enemies within the specified radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in colliders)
        {
            // Check if the collider belongs to an enemy
            if (collider.CompareTag("Enemy"))
            {
                // Add the enemy's transform to the list
                enemies.Add(collider.transform);
            }
        }

        // Select the next enemy if any enemies were found
        if (enemies.Count > 0)
        {
            // Increment the target index
            currentTargetIndex++;

            // Wrap around to the beginning if the index exceeds the list length
            if (currentTargetIndex >= enemies.Count)
            {
                currentTargetIndex = 0;
            }

            // Update the current target reference
            currentTarget = enemies[currentTargetIndex];
            underTargetVFX.SetActive(true);
            underTargetVFX.transform.position = currentTarget.position + Vector3.down * (currentTarget.localScale.y / 2);
        }
        else
        {
            underTargetVFX.SetActive(false);
            currentTargetIndex = -1;
            currentTarget = null;
            Debug.Log("No enemies found");
        }
    }

    public Transform GetTarget()
    {
        return currentTarget;
    }
}
