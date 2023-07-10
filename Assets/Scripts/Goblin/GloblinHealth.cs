using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloblinHealth : MonoBehaviour
{
    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (other.GetComponent<SimpleProjectile>().enemyTag != "Player")
            {
                m_Animator.SetTrigger("Die");
                GetComponent<GoblinMovement>().isDead = true;
            }

        }
    }
}
