using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttack : MonoBehaviour
{
    [SerializeField] private GameObject slashAttack;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        Instantiate(slashAttack, transform.position, transform.rotation);
    }
}
