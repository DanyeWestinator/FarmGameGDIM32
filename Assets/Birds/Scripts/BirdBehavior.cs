using System.Collections;
using System.Collections.Generic;
using Pathfinding.Examples;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// Weird pseudo-state machine that defines transitions and behavior states for birds
/// </summary>
public class BirdBehavior : AIBehavior
{
    
    public CatBehavior cat; // set by spawner
    
    public float safeDistance = 100;
    [Tooltip("The time between updating bird states")]
    [SerializeField] private float stateUpdateTime = 0.5f;

    [SerializeField] private float destroyPlantTime = 1f;
    [SerializeField] private float eatingDistance = .05f;
    [SerializeField] private float warningDistance = 2f;
    private bool isWarning = false;

    private Plant currentPlant = null;

    private AstarSmoothFollow2 follower;
    [SerializeField]
    private Transform targetChild;

    [SerializeField] private Animator anim;

    private PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        follower = GetComponent<AstarSmoothFollow2>();
        
        if (targetChild == null)
            targetChild = transform.Find("target");
        follower.target = targetChild;
        StartCoroutine(checkState());

        player = PlayerController.player;
    }
    /// <summary>
    /// Checks which state the bird should currently be in. Doesn't run every frame for performance
    /// </summary>
    /// <returns></returns>
    IEnumerator checkState()
    {
        Vector3 pos = transform.position;
        if (math.abs(pos.x) >= 20 || math.abs(pos.y) >= 20)
            Destroy(gameObject);
        while (true)
        {
            yield return new WaitForSeconds(stateUpdateTime);
            //If the player is close, flee
            if (distanceToThreat(player.gameObject) <= safeDistance)
            {
                flee(player.gameObject);
            }
            // If cat is close, flee
            else if (distanceToThreat(cat.gameObject) <= safeDistance)
            {
                flee(cat.gameObject);
            }
            //Go to plant if there are any
            else if (CanGoToPlant())
            {
            }
            //Else idle
            else
            {
                Idle();
            }
        }
    }
    /// <summary>
    /// Sends the bird running from the player
    /// </summary>
    private void flee(GameObject threat)
    {
        follower.target = targetChild;
        Vector3 dir = transform.position - threat.transform.position;
        dir = dir.normalized * safeDistance;
        targetChild.localPosition = dir;
        if (currentPlant)
        {
            currentPlant.RemoveEmote();
        }

        isWarning = false;

    }
    /// <summary>
    /// Checks if the bird can go to the nearest plant. Sets target if true
    /// </summary>
    /// <returns>Returns if the bird is going towards a plant</returns>
    bool CanGoToPlant()
    {
        Vector3 pos = transform.position;
        Plant p = FarmSpawner.findClosestPlant(pos);
        if (p == null)
        {
            return false;
        }

        currentPlant = p;
        //Set our target to be the plant
        follower.target = p.transform.parent;
        //Reset the target child to self
        targetChild.localPosition = Vector3.zero;
        float distance = Vector2.Distance(follower.target.position, pos);
        if (distance <= warningDistance && isWarning == false)
        {
            isWarning = true;
            currentPlant.Emote("bird yellow");
        }
        //If we're at the plant, start eating the plant
        if (distance <= eatingDistance && startedDestroying == null)
        {
            isWarning = false;
            startedDestroying = startDestroyPlant();
            StartCoroutine(startedDestroying);
        }
        return true;
    }
    /// <summary>
    /// Idling logic
    /// </summary>
    void Idle()
    {
        targetChild.localPosition = Vector3.zero;
        follower.target = targetChild;
    }
    /// <summary>
    /// How close the bird is to a given threat
    /// </summary>
    /// <param name="threat">The GO of the threat to check</param>
    /// <returns>Returns Vector2.distance (ignores Z) of ourself to the threat</returns>
    private float distanceToThreat(GameObject threat)
    {
        return Vector2.Distance(transform.position, threat.transform.position);
    }

    public void Catch(CatBehavior cat)
    {
        Destroy(gameObject);
    }

    private IEnumerator startedDestroying = null;
    IEnumerator startDestroyPlant()
    {
        currentPlant.Emote("bird red");
        yield return new WaitForSeconds(destroyPlantTime);
        //If there is a plant on what we're following after the cooldown
        if (currentPlant != null)
        {
            currentPlant.Die();
        }
        
    }




    /*OLD BIRD BEHAVIOR
     // STATE TRIGGERS:

    public override void OnDetected(GameObject gameObject)
    {
        return;
        base.OnDetected(gameObject);
        
        // if cat, run!
        if (gameObject.GetComponent<CatBehavior>())
        {
            //setRunAway(gameObject);
        }
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
        //AIMover.TargetDestination = escapeTarget;
        AIMover.MoveVelocity = runAwaySpeed;
    }

    private void setFeed(GameObject target)
    {
        // set mover
        AIMover.TargetDestination = target.transform;
        AIMover.MoveVelocity = descendSpeed;
    }
    /// <summary>
    /// OLD BIRD BEHAVIOR
    /// </summary>
    /// 
    */

}
