using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovingAI
{
    Vector3 TargetDestination {get; set;}
    float MoveVelocity {get; set;}
}
