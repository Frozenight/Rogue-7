using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeTrigger : MonoBehaviour
{
    [SerializeField] private RoomController roomToTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            roomToTrigger.StopAIPhasing();
    }
}
