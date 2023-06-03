using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static event Action OnAIPositiveTrigger;
    public static event Action OnAINegativeTrigger;


    private void Start()
    {
        instance = this;
    }
    public void StartAIPhasing()
    {
        OnAIPositiveTrigger?.Invoke();
    }

    public void StopAIPhasing()
    {
        OnAINegativeTrigger?.Invoke();
    }
}
