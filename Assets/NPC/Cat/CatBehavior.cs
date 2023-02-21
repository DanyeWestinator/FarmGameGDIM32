using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nya
/// </summary>
public class CatBehavior : AIBehavior
{
    
    // whatever the cat is currently looking at
    public GameObject fixation;
    
    void Start()
    {
        setChase(fixation);
    }

    void Update()
    {
        manageBehavior();
    }


    // STATE TRIGGERS:

    public override void OnDetected(GameObject gameObject)
    {
        base.OnDetected(gameObject);
        
    }

    public void BirdCaught(GameObject bird)
    {

    }

    public void FeedMe(GameObject player, GameObject food)
    {

    }


    // BEHAVIOR STATES AND MANAGEMENT:
    // constant behavior states

    enum CatBehaviorState {IDLE, CHASE, NOTICE, RUNAWAY}
    private CatBehaviorState state = CatBehaviorState.IDLE;

    // call in update()
    private void manageBehavior()
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
        AIMover.TargetDestination = chaseTarget.transform.position;
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
