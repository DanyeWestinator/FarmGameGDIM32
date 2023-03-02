using System.Collections;
using System.Collections.Generic;
using Pathfinding.Examples;
using UnityEngine;

/// <summary>
/// 
/// Nya ~~
///
/// this class is very hacked together
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
    public CatEmoter emoter;
    public AstarSmoothFollow2 follower;
    public Animator animator;
    public GameObject CatHome;
    public float RunSpeed = 5;
    public float WalkSpeed = 2;
    public float SafeDistance = 2;
    public float CatchDistance = 1.2f;
    public int RelationshipThreshold = 5; 
    
    // once bird gets this far away, gives up chase
    public float GiveUpDistance = 10;

    // chance to run away from low-relation player 
    // multiplied by closeness to threshold
    public float runawayChance = 0.5f; 

    // return to idle after running
    public float IdleTimer = 1;

    // return to home after idling
    public float HomeTimer = 10;
    
    // hunt cooldown
    public float HuntTimer = 5; 

    // nap timer (before hunting again)
    public float NapTimer = 5;
    

    // private params


    private int playerRelationship = 0;
    private bool huntOnCooldown = false;
    private List<BirdBehavior> birds = new List<BirdBehavior>();
    private Coroutine currentTimer = null; // wow this is hacky


    // whatever the cat is currently looking at (target)
    public GameObject fixation;
    
    void Start()
    {
        emoter.hide();
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
           print("Cat hit bird");
           BirdCaught(birdBehavior);
        }

        // if player (and not chasing), notice
        if (gameObject.GetComponent<PlayerController>())
        {
            if (!(state == CatBehaviorState.CHASE))
            {
                setNoticePlayer(gameObject);
            }
        }
    }

    public void BirdCaught(BirdBehavior bird)
    {
        bird.Catch(this);
        birds.Remove(bird);
        setIdle();
        print("bird caught");
    }

    public void BirdEscaped()
    {
        // stop chasing
        follower.target = createBall(transform.position).transform;
        setIdle();
        print("bird escaped");
    }

    public void AddNewBird(BirdBehavior bird)
    {
        birds.Add(bird);
    }

    public void FeedMe(GameObject player, GameObject food)
    {
        playerRelationship++;
        print("CAT FED: " + playerRelationship);
        emoter.display("emote_heart");
        currentTimer = StartCoroutine(idleDelay());
    }

    public void TryHunt()
    {
        if (birds.Count <= 0) return;

        // get closest bird
        BirdBehavior closest = birds[0];
        var cldistance = Vector3.Distance(transform.position, closest.transform.position);
        foreach (BirdBehavior bird in birds)
        {
            var distance = Vector3.Distance(transform.position, bird.transform.position);
            if (distance < cldistance) 
            {
                closest = bird;
                cldistance = distance;
            } 
        }

        setChase(closest.gameObject);
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

    IEnumerator homeDelay()
    {
        yield return new WaitForSeconds(HomeTimer);
        setHome();
    }

    IEnumerator napDelay()
    {
        yield return new WaitForSeconds(NapTimer);
        setIdle();
    }

    // get a quick transform to track this is really bad
    private GameObject createBall(Vector3 pos)
    {
        var ball = new GameObject("target");
        ball.transform.position = pos;

        // print("new ball at: " + pos);

        return ball;
    }

    private void stopCoroutines()
    {
        if (currentTimer != null)
        {
            StopCoroutine(currentTimer);
        } 
    }



    // =========== BEHAVIOR STATES AND MANAGEMENT: ===========
    // for per-frame behavior



    enum CatBehaviorState {IDLE, CHASE, NOTICE, RUNAWAY, HOME}
    private CatBehaviorState state = CatBehaviorState.IDLE;

    // called in update()
    protected override void manageBehavior()
    {
        
        switch (state)
        {
            case CatBehaviorState.IDLE:
                break;
        
            case CatBehaviorState.CHASE:

                print("cat is chasing: " + fixation);
                // give up if too far
                var distance = Vector3.Distance(transform.position, fixation.transform.position);
                print(distance);
                if (distance > GiveUpDistance)
                {
                    BirdEscaped();
                }

                // doing this here because the detector isn't picking up birds 
                // and this is due today
                if (distance <= CatchDistance)
                {
                    BirdCaught(fixation.GetComponent<BirdBehavior>());
                }

                break;

            case CatBehaviorState.NOTICE:
                break;
            case CatBehaviorState.RUNAWAY:
                break;
            case CatBehaviorState.HOME:
                
                var target = CatHome.transform.position;
                var toTarget = target - transform.position;
                if (toTarget.magnitude < 0.2)
                {
                    setIdle();
                }

                break;
        }

    }



    // ========== SET BEHAVIOR STATES ==========
    // for one time behaviors 



    private void setIdle()
    {
        state = CatBehaviorState.IDLE;

        stopCoroutines();

        emoter.hide();

        animator.ResetTrigger("Walk");
        animator.SetTrigger("Idle");

        print("cat set idle");

        // stop moving
        follower.target = createBall(transform.position).transform;

        // either go home or hunt
        if (huntOnCooldown)
        {
            print("hunt on cooldown, cat going home");
            currentTimer = StartCoroutine(homeDelay());
        }
        else
        {
            print("cat trying to hunt");
            TryHunt();
        }
        
    }
    
    private void setChase(GameObject chaseTarget)
    {
        state = CatBehaviorState.CHASE;

        fixation = chaseTarget;

        stopCoroutines();

        // set chase target and speed in mover
        follower.target = chaseTarget.transform;
        follower.damping = RunSpeed;

        // reset cooldown 
        huntOnCooldown = true;
        StartCoroutine(huntDelay());

        emoter.display("emote_exclamation");

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");

        print("cat hunting: " + chaseTarget);
    }

    private void setNoticePlayer(GameObject player)
    {
        state = CatBehaviorState.NOTICE;

        stopCoroutines();

        print("cat noticed player  at: " + follower.transform.position);

        follower.target = createBall(transform.position).transform;

        // might run away if at low relationship
        if (playerRelationship <= RelationshipThreshold)
        {
            var dif = RelationshipThreshold - playerRelationship;
            float ratio = ((float)dif) / RelationshipThreshold;
            var chance = ratio * runawayChance;

            float rand = Random.Range(0f,1f);

            // print("cat run away? ratio: " + rand + " / " + chance);
            // print("ratio: " + ratio + " dif " + dif);

            if (rand <= chance)
            {
                setRunAway(player);
                return;
            }
        } 

        emoter.display("emote_weary");

        // chill after a while
        currentTimer = StartCoroutine(idleDelay());


    }

    private void setRunAway(GameObject player)
    {
        // on runaway set, runs a set distance away from the player 
       
        state = CatBehaviorState.RUNAWAY;

        stopCoroutines();

        // path away from player * safedistance
        var toPlayer = player.transform.position - transform.position;
        var target = toPlayer.normalized * -SafeDistance;

        var ball = createBall(target);

        follower.target = ball.transform;
        follower.damping = RunSpeed;

        print("cat running away: " + target);

        // chill after a while
        currentTimer = StartCoroutine(idleDelay());

        emoter.display("emote_exclamation");

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");

    }


    private void setHome()
    {
        stopCoroutines();
       
        print("cat returning home to: " + CatHome.transform.position);
        state = CatBehaviorState.HOME;
        follower.target = CatHome.transform;
        follower.damping = WalkSpeed;

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");

    }

    
    

}
