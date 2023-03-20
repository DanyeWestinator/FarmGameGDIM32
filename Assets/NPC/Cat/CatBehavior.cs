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
    [SerializeField] private FarmSpawner farm;
    [SerializeField] private CatEmoter emoter;
    [SerializeField] private AstarSmoothFollow2 follower;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem heartParticles;

    public GameObject CatHome;

    [SerializeField] private float RunSpeed = 5;
    [SerializeField] private float WalkSpeed = 2;
    [SerializeField] private float SafeDistance = 2;
    [SerializeField] private float CatchDistance = 1.2f;
    [SerializeField] private float CatnipDistance = 0.4f;
    [SerializeField] private int RelationshipThreshold = 5; 
    
    // once bird gets this far away, gives up chase
    [SerializeField] private float GiveUpDistance = 10;

    // chance to run away from low-relation player 
    // multiplied by closeness to threshold
    [SerializeField] private float runawayChance = 0.5f; 

    // return to idle after running
    [SerializeField] private float IdleTimer = 1;

    // return to home after idling
    [SerializeField] private float HomeTimer = 10;
    
    // hunt cooldown
    [SerializeField] private float HuntTimer = 5; 

    // nap timer (before hunting again)
    [SerializeField] private float NapTimer = 5;
    

    // private params


    private int playerRelationship = 0;
    private bool huntOnCooldown = false;
    private List<BirdBehavior> birds = new List<BirdBehavior>();
    private Plant currentCatnip = null;
    private Coroutine currentTimer = null; // for stopping coroutines wow this is hacky
    private GameObject previousTarget = null; // for deletion, also hacky


    // whatever the cat is currently looking at (target)
    private GameObject fixation;
    
    void Start()
    {
        emoter.hide();
        idleDelay();
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

        // if harvestable catnip,
        var asPlant = GetComponent<Catnip>();
        if (asPlant && asPlant.Harvestable)
        {
            CatnipFound(asPlant);
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

    public void CatnipFound(Plant catnip)
    {
        print("cat harvesting catnip");
        currentCatnip = null;

        catnip.Harvest();
        heartParticles.Play();
        
        playerRelationship++;
        setIdle();
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
        if (birds.Count <= 0) 
        {
            currentTimer = StartCoroutine(idleDelay());
            return;
        }

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

        setChase(closest.gameObject, RunSpeed);
    }

    public Plant TryFindCatnip()
    {
        var harvestable = FarmSpawner.getHarvestablePlants();

        foreach (var v in harvestable)
        {
            if (v.GetComponent<Catnip>())
            {
                return v;
            }
        }

        return null;
    }

    // OTHER HELPERS ==========================================

    // get a quick transform to track this is really bad
    private GameObject createBall(Vector3 pos)
    {
        Destroy(previousTarget);
        
        var ball = new GameObject("target");
        ball.transform.position = pos;

        // print("new ball at: " + pos);
        previousTarget = ball;

        return ball;
    }

    private float relationshipRatio()
    {
        return (float)playerRelationship / RelationshipThreshold;
    }

    // coroutine helpers =============================
    
    // switches to idle 
    IEnumerator idleDelay()
    {
        yield return new WaitForSeconds(IdleTimer);
        setIdle();
    }

    IEnumerator huntDelay()
    {
        // shorter delay if better relationship
        yield return new WaitForSeconds(HuntTimer * (1 - relationshipRatio()));
        huntOnCooldown = false;
    }

    IEnumerator homeDelay()
    {
        // shorter delay if better relationship
        yield return new WaitForSeconds(HomeTimer * (1 - relationshipRatio()));
        setHome();
    }

    IEnumerator napDelay()
    {
        yield return new WaitForSeconds(NapTimer);
        setIdle();
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



    enum CatBehaviorState {IDLE, CHASE, NOTICE, RUNAWAY, HOME, CATNIP}
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
                var distance = Vector3.Distance(transform.position, fixation.transform.position);
                if (distance > GiveUpDistance)
                {
                    BirdEscaped();
                }

                // doing this here because the detector isn't picking up birds 
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

            case CatBehaviorState.CATNIP:
                
                if (currentCatnip == null || currentCatnip.IsDead)
                {
                    setIdle();
                    break;
                }

                // sigh
                var catnip_distance = Vector3.Distance(transform.position, currentCatnip.transform.position);
                if (catnip_distance <= CatnipDistance) CatnipFound(currentCatnip);

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

        //print("cat set idle");

        // stop moving
        follower.target = createBall(transform.position).transform;

        // check for catnip for good measure
        var catnip = TryFindCatnip();
        if (catnip)
        {
            setCatnip(catnip);
            return;
        }

        // either go home or hunt
        if (huntOnCooldown)
        {
            //print("hunt on cooldown, cat going home");
            currentTimer = StartCoroutine(homeDelay());
        }
        else
        {
            //print("cat trying to hunt");
            TryHunt();
        }
        
    }
    
    private void setChase(GameObject chaseTarget, float speed, string emote = "emote_exclamation")
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

        emoter.display(emote);

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");

        //print("cat hunting: " + chaseTarget);
    }

    private void setNoticePlayer(GameObject player)
    {
        state = CatBehaviorState.NOTICE;
        stopCoroutines();

        //print("cat noticed player  at: " + follower.transform.position);

        follower.target = createBall(transform.position).transform;

        // might run away if at low relationship
        if (playerRelationship <= RelationshipThreshold)
        {
            emoter.display("emote_weary");
            
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
        else
        {
            emoter.display("emote_heart");
        } 

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

        //print("cat running away: " + target);

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

        // start nap timer (then back to idle)
        currentTimer = StartCoroutine(napDelay());

    }

    
    // path to catnip
    private void setCatnip(Plant catnip)
    {       
        stopCoroutines();
        state = CatBehaviorState.CATNIP;

        currentCatnip = catnip;
        fixation = catnip.gameObject;

        follower.target = createBall(catnip.transform.position).transform;
        follower.damping = WalkSpeed;

        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");

        emoter.display("emote_heart");

        // only exits when catnip found or destroyed
    }

}
