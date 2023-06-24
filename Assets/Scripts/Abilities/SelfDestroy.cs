using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    void Start()
    {
        StartCoroutine(DestroyItself());
    }

    IEnumerator DestroyItself()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
