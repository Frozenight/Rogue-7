using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float speed = 3.0f;
    public float changeDirectionInterval = 2.0f;
    private Vector3 movement;
    private CharacterController characterController;
    private ColorReader colorReader;
    private MeshRenderer meshRenderer;

    private bool isPhasing = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        colorReader = GetComponent<ColorReader>();
        meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
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
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionInterval);
            ChangeDirectionNow();
        }
    }

    private void ChangeDirectionNow()
    {
        float randomAngle = Random.Range(0f, 360f);
        movement = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle));
    }

    public void StartPhasing()
    {
        isPhasing = true;
    }

    public void StopPhasing()
    {
        isPhasing = false;
    }

    private void OnEnable()
    {
        GameController.OnAIPositiveTrigger += StartPhasing;
        GameController.OnAINegativeTrigger += StopPhasing;
    }

    private void OnDisable()
    {
        GameController.OnAIPositiveTrigger -= StartPhasing;
        GameController.OnAINegativeTrigger -= StopPhasing;

    }
}
