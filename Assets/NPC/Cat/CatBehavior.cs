using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Nya ~~
///
/// this class is very badly organized
///
/// Behavior:
///
/// will chase birds when notified by public newBird 
/// (birdspawner holds reference to cat)
/// detection is 'personal space'
/// when player enters detection, may run away
/// when bird enters detection, bird is caught
/// 
/// </summary>
public class CatBehavior : AIBehavior
{
    // public params
    public float RunSpeed = 5;
    public float WalkSpeed = 2;
    public float SafeDistance = 2;
    public int RelationshipThreshold = 5; 
    
    // once bird gets this far away, gives up chase
    public float GiveUpDistance = 4;

    // chance to run away from low-relation player 
    // multiplied by closeness to threshold
    public float runawayChance = 0.5f; 

    // return to idle after running
    public float IdleTimer = 1;
    
    // hunt cooldown
    public float HuntTimer = 5; 

    // emote sprites
    public Sprite emote;
    


    private int playerRelationship = 0;
    private bool huntOnCooldown = false;


    // whatever the cat is currently looking at (target)
    public GameObject fixation;
    
    void Start()
    {
        
    }

    void Update()
    {
        manageBehavior();
    }


    // =========== STATE TRIGGERS / BEHAVIOR FUNCTIONS: ============
    // legible functions that interact with behavior states


    public override void OnDetected(GameObject gameObject)
    {
        base.OnDetected(gameObject);
        
        // if bird, catch bird
        var birdBehavior = gameObject.GetComponent<BirdBehavior>();
        if (birdBehavior)
        {
           birdBehavior.Catch(this);
           BirdCaught(gameObject);
        }

        // if player, notice
        if (gameObject.GetComponent<PlayerController>())
        {
            setNoticePlayer(gameObject);
        }
    }

    public void BirdCaught(GameObject bird)
    {
        setIdle();
    }

    public void BirdEscaped()
    {
        setIdle();
    }

    public void AddNewBird(GameObject bird)
    {
        // TODO logic for multiple birds
    }

    public void FeedMe(GameObject player, GameObject food)
    {
        playerRelationship++;
        print("CAT FED: " + playerRelationship);
    }

    // coroutine helpers
    
    // switches to idle 
    IEnumerator idleDelay()
    {
        yield return new WaitForSeconds(IdleTimer);
        setIdle();
    }

    IEnumerator huntDelay()
    {
        yield return new WaitForSeconds(HuntTimer);
        huntOnCooldown = false;
    }




    // =========== BEHAVIOR STATES AND MANAGEMENT: ===========
    // for per-frame behavior


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

                // give up if too far
                // else movement update target
                Vector3 target = fixation.transform.position;
                Vector3 toTarget = target - transform.position;
                if (toTarget.magnitude > GiveUpDistance)
                {
                    BirdEscaped();
                }
                else
                {
                    AIMover.TargetDestination = fixation.transform;
                }

                break;

            case CatBehaviorState.NOTICE:
                break;
            case CatBehaviorState.RUNAWAY:
                break;
        }

    }


    // ========== SET BEHAVIOR STATES ==========
    // for one time behaviors 


    private void setIdle()
    {
        state = CatBehaviorState.IDLE;

        // stop chasing
        AIMover.TargetDestination = transform;
    }
    
    private void setChase(GameObject chaseTarget)
    {
        state = CatBehaviorState.CHASE;

        fixation = chaseTarget;

        // set chase target and speed in mover
        AIMover.TargetDestination = chaseTarget.transform;
        AIMover.MoveVelocity = RunSpeed;

        // reset cooldown 
        huntOnCooldown = true;
        StartCoroutine(huntDelay());
    }

    private void setNoticePlayer(GameObject player)
    {
        state = CatBehaviorState.NOTICE;

        fixation = player;

        // maybe run away if at low relationship
        if (playerRelationship <= RelationshipThreshold)
        {
            var dif = RelationshipThreshold - playerRelationship;
            var ratio = dif / RelationshipThreshold;
            var chance = ratio * runawayChance;

            float rand = Random.Range(0f,1f);

            print("cat run away? ratio: " + rand + " / " + chance);

            if (rand <= chance)
            {
                setRunAway(player);
                return;
            }
        } 

        // chill after a while
        StartCoroutine(idleDelay());


    }

    private void setRunAway(GameObject player)
    {
        // on runaway set, runs a set distance away from the player 
       
        state = CatBehaviorState.RUNAWAY;

        // path away from player * safedistance
        var toPlayer = player.transform.position - transform.position;
        var target = toPlayer.normalized * -SafeDistance;

        var ball = new GameObject("target");
        ball.transform.position = target;

        AIMover.TargetDestination = ball.transform;
        AIMover.MoveVelocity = RunSpeed;

        // chill after a while
        StartCoroutine(idleDelay());
    }

    
    

}
