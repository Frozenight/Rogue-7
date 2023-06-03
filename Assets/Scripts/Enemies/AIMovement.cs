using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public float turnSpeed = 90f; // speed of turning in degrees per second
    private Vector3 movement;
    private CharacterController characterController;
    private ColorReader colorReader;
    private MeshRenderer meshRenderer;

    private Coroutine changeDirectionCoroutine;

    private bool isPhasing = false;
    private bool isMoving = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        colorReader = GetComponent<ColorReader>();
        meshRenderer = GetComponent<MeshRenderer>();
        ChangeDirectionNow();
    }

    private void Update()
    {
        if (!isMoving)
            return;
        characterController.SimpleMove(movement * speed);
        if (isPhasing & colorReader.shouldBePhased)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            if (!meshRenderer.enabled)
                meshRenderer.enabled = true;
        }
        TurnToFaceMovementDirection();
    }

    private void TurnToFaceMovementDirection()
    {
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            ChangeDirectionNow();
        }
    }

    private void ChangeDirectionNow()
    {
        float randomAngle = Random.Range(0f, 360f);
        movement = new Vector3(Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0, Mathf.Cos(randomAngle * Mathf.Deg2Rad));
    }

    public void StartPhasing()
    {
        changeDirectionCoroutine = StartCoroutine(ChangeDirection());
        isPhasing = true;
        isMoving = true;
    }

    public void StopPhasing()
    {
        if (changeDirectionCoroutine != null)
        {
            StopCoroutine(changeDirectionCoroutine);
            changeDirectionCoroutine = null;
        }

        isPhasing = false;
        isMoving = false;
    }
}

