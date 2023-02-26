using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Weird pseudo-state machine that defines transitions and behavior states for birds
/// </summary>
public class BirdBehavior : AIBehavior
{
    public float descendSpeed = 4;
    public float runAwaySpeed = 7;
    public float runAwayDeviation = Mathf.PI / 2;
    public float safeDistance = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        AIMover.TargetDestination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // temp
        if (Input.GetButtonDown("Fire2"))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            AIMover.TargetDestination = pos;
            AIMover.MoveVelocity = descendSpeed;
        }
    }


    // STATE TRIGGERS:

    public override void OnDetected(GameObject gameObject)
    {
        base.OnDetected(gameObject);
        
        // if cat, run!
        if (gameObject.GetComponent<CatBehavior>())
        {
            setRunAway(gameObject);
        }
    }

    public void Catch(CatBehavior cat)
    {
        Destroy(this);
    }


    // BEHAVIOR STATES AND MANAGEMENT:
    // for every-frame behavior

    enum BirdBehaviorState {IDLE, RUNAWAY, FEED}
    private BirdBehaviorState state = BirdBehaviorState.IDLE;

    // called in update()
    protected override void manageBehavior()
    {
        
        switch (state)
        {
            case BirdBehaviorState.IDLE:
                break;
            case BirdBehaviorState.RUNAWAY:
                break;
            case BirdBehaviorState.FEED:
                break;
        }

    }


    // SET BEHAVIOR STATES
    // for changes when behavior states are set

    private void setIdle()
    {
        state = BirdBehaviorState.IDLE;
    }
    
    private void setRunAway(GameObject chaser)
    {
        state = BirdBehaviorState.RUNAWAY;

        // set escape target as opposite of chaser * safedistance
        var chaserPosition = chaser.transform.position;
        var escapeTarget = transform.position - chaserPosition * -1f * safeDistance;
        
        // set mover
        AIMover.TargetDestination = escapeTarget;
        AIMover.MoveVelocity = runAwaySpeed;
    }

    private void setFeed(GameObject target)
    {
        // set mover
        AIMover.TargetDestination = target.transform.position;
        AIMover.MoveVelocity = descendSpeed;
    }

}
