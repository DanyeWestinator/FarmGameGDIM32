using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// moves the cat nya 
/// put me on the cat uwu
/// </summary>
public class CatMover : AIMover
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
    /*
    // temp - moves the cat to target
    private void move()
    {
        if (!TargetDestination) return;

        var dir = TargetDestination.position - transform.position;
        var step = dir.normalized * MoveVelocity * Time.deltaTime; 
        transform.position += step;
    }*/
    
}
