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
    public int RelationshipThreshold = 5; 
    
    // once bird gets this far away, gives up chase
    public float GiveUpDistance = 4;

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
           BirdCaught(birdBehavior);
        }

        // if player, notice
        if (gameObject.GetComponent<PlayerController>())
        {
            setNoticePlayer(gameObject);
        }
    }

    public void BirdCaught(BirdBehavior bird)
    {
        bird.Catch(this);
        birds.Remove(bird);
        setIdle();
    }

    public void BirdEscaped()
    {
        // stop chasing
        follower.target = createBall(transform.position).transform;
        setIdle();
    }

    public void AddNewBird(BirdBehavior bird)
    {
        birds.Add(bird);
        setChase(bird.gameObject);
    }

    public void FeedMe(GameObject player, GameObject food)
    {
        playerRelationship++;
        print("CAT FED: " + playerRelationship);
        emoter.display("emote_heart");
        currentTimer = StartCoroutine(idleDelay());
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
        // TODO
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

                // give up if too far
                Vector3 target = fixation.transform.position;
                Vector3 toTarget = target - transform.position;
                if (toTarget.magnitude > GiveUpDistance)
                {
                    BirdEscaped();
                }

                break;

            case CatBehaviorState.NOTICE:
                break;
            case CatBehaviorState.RUNAWAY:
                break;
            case CatBehaviorState.HOME:
                
                target = CatHome.transform.position;
                toTarget = target - transform.position;
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

        // stop
        follower.target = createBall(transform.position).transform;

        // get ready to go home
        currentTimer = StartCoroutine(homeDelay());
        print("Cat returning idle");
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
        currentTimer = StartCoroutine(huntDelay());

        emoter.display("emote_exclamation");

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");
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
