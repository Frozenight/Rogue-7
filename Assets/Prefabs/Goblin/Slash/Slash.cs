using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public float speed = 5f; // Speed of the object's movement
    private float moveDuration = 2f; // Duration of the object's movement
    private float lifetime = 3f; // Time before the object is destroyed

    private bool isMoving = true;
    [SerializeField] private BoxCollider collider;

    private void Start()
    {
        StartCoroutine(MoveAndDestroy());
    }

    private IEnumerator MoveAndDestroy()
    {
        yield return new WaitForSeconds(moveDuration);
        isMoving = false;
        collider.enabled = false;
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (isMoving)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
