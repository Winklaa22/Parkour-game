using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    // Enter
    public delegate void OnEnter(Collider col);
    public OnEnter Entity_OnEnter;

    // Exit
    public delegate void OnExit(Collider col);
    public OnExit Entity_OnExit;
    
    private void OnTriggerEnter(Collider col) => Entity_OnEnter?.Invoke(col);
    private void OnTriggerExit(Collider col) => Entity_OnExit?.Invoke(col);

}
