using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nya
/// </summary>
public class CatBehavior : AIBehavior
{
    // adjustable stuff
    public float ChaseSpeed = 5;
    public float CuriousSpeed = 2;



    // whatever the cat is currently looking at
    public GameObject fixation;
    
    void Start()
    {
        
    }

    void Update()
    {
        manageBehavior();
    }


    // STATE TRIGGERS:

    public override void OnDetected(GameObject gameObject)
    {
        base.OnDetected(gameObject);
        
        // if bird, chase!
        if (gameObject.GetComponent<BirdBehavior>())
        {
            setChase(gameObject);
        }
    }

    public void BirdCaught(GameObject bird)
    {

    }

    public void FeedMe(GameObject player, GameObject food)
    {

    }


    // BEHAVIOR STATES AND MANAGEMENT:
    // for every-frame behavior

    enum CatBehaviorState {IDLE, CHASE, NOTICE, RUNAWAY}
    private CatBehaviorState state = CatBehaviorState.IDLE;

    // called in update()
    protected override void manageBehavior()
    {
        
        switch (state)
        {
            case CatBehaviorState.IDLE:
                break;
            case CatBehaviorState.CHASE:
                break;
            case CatBehaviorState.NOTICE:
                break;
            case CatBehaviorState.RUNAWAY:
                break;
        }

    }


    // SET BEHAVIOR STATES
    // for changes when behavior states are set

    private void setIdle()
    {
        state = CatBehaviorState.IDLE;
    }
    
    private void setChase(GameObject chaseTarget)
    {
        state = CatBehaviorState.CHASE;

        // set chase target and speed in mover
        AIMover.TargetDestination = chaseTarget.transform.position;
        AIMover.MoveVelocity = ChaseSpeed;
    }

    private void setNoticePlayer(GameObject player)
    {
        state = CatBehaviorState.NOTICE;
    }

    private void setRunAway(GameObject player)
    {
        state = CatBehaviorState.RUNAWAY;
    }



}
