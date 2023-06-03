using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader : MonoBehaviour
{
    [SerializeField] private Material material;
    // Update is called once per frame
    void Update()
    {
        // Change the property based on time
        float newValue = Mathf.PingPong(Time.time, 1.0f);
        material.SetFloat("_Sharpness", newValue);
    }
}
