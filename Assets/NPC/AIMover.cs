using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMover: MonoBehaviour
{
    
    public Vector3 TargetDestination;
    [HideInInspector] public float MoveVelocity;
}
