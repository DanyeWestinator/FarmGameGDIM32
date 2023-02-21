using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// moves the cat nya 
/// put me on the cat uwu
/// </summary>
public class CatMover : MonoBehaviour, IMovingAI
{
    public Vector3 TargetDestination
    {
        get{ return TargetDestination; }
        set{ TargetDestination = value; }
    }

    public float MoveVelocity
    {
        get{ return MoveVelocity; }
        set{ MoveVelocity = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // temp - moves the cat to target
    private void move()
    {
        var dir = TargetDestination - transform.position;
        var step = dir.normalized * MoveVelocity * Time.deltaTime; 
        transform.position += step;
    }
    
}
