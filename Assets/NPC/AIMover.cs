using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIMover: MonoBehaviour
{
    
    [HideInInspector] public Transform TargetDestination;
    [HideInInspector] public float MoveVelocity;
}
