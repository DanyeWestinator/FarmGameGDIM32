using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMover : AIMover
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    // temp - moves the cat to target
    private void move()
    {
        var dir = TargetDestination - transform.position;
        var step = dir.normalized * MoveVelocity * Time.deltaTime; 
        transform.position += step;
    }
}
