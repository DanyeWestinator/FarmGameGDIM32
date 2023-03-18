using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMover : AIMover
{
    void Update()
    {
        move();
    }

    // temp - moves the cat to target
    private void move()
    {
        if (TargetDestination == null)
        {
            
            return;
        }

        Vector3 pos = transform.position;
        var dir = TargetDestination.position - pos;
        var step = dir.normalized * (MoveVelocity * Time.deltaTime);
        step.z = 0f;
        transform.position = pos + step;
        //print($"Moved bird {dir}, or step? {step}. Transform position is now {transform.position}");
    }
}
