using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // Calculate the desired rotation to face the camera
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position, Vector3.up);

        // Apply the rotation to the quad
        transform.rotation = lookRotation;
    }
}
