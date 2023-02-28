using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMover: MonoBehaviour
{
    
    [HideInInspector] public Transform TargetDestination;
    [HideInInspector] public float MoveVelocity;
    
    protected void move()
    {
        if (TargetDestination == null)
            return;
        var dir = TargetDestination.position - transform.position;
        var step = dir.normalized * MoveVelocity * Time.deltaTime; 
        transform.position += step;
    }
}
