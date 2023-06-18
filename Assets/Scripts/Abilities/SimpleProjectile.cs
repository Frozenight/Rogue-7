using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 targetPosition;
    public GameObject handPoistion;
    private bool release = false;

    private void Update()
    {
        if (targetPosition != Vector3.zero && release)
        {
            MoveTowardsTarget();
        }
        else
        {
            transform.position = handPoistion.transform.position;
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    public void ReleaseProjectile()
    {
        release = true;
    }
}
