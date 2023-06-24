using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PulseAOE : GroundAOE
{
    [SerializeField] private VisualEffect vfx;
    private bool expand;
    private float expandRate = 5f;
    private float currentSize;
    private float additionalLifeTime = 1f;
    private void Update()
    {
        if (!expand)
            transform.position = handPosition.transform.position;
        if (expand && currentSize < 1f)
        {
            currentSize += expandRate * Time.deltaTime;
            vfx.SetFloat("Size", currentSize);
        }
        duration -= Time.deltaTime;
        if (duration + additionalLifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public override void Expand()
    {
        base.Expand(); // Call the base class implementation first
        currentSize = vfx.GetFloat("Size");
        expand = true;
    }
}
