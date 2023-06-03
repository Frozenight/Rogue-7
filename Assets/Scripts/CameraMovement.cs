using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0, 2, -3);
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
