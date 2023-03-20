using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for AI behavior
/// </summary>
public class AIBehavior: MonoBehaviour
{
    protected Transform target;

    void Update()
    {
        manageBehavior();
    }

    public virtual void OnDetected(GameObject gameObject)
    {
        
    }

    // called every frame - maange behavior here
    protected virtual void manageBehavior()
    {

    }
}
