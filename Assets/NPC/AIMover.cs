using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMover: MonoBehaviour
{
    
    [HideInInspector] public Vector3 TargetDestination;
    [HideInInspector] public float MoveVelocity;
}
