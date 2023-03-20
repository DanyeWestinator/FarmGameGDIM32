using System.Collections;
using System.Collections.Generic;
using Pathfinding.Examples;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// Controls the logic for the AI Player
/// </summary>
public class AIPlayerBehavior : AIBehavior
{
    [SerializeField] private float stateUpdateTime = 0.5f;
    /// <summary>
    /// How close the AI player needs to get to an object to consider it "reached"
    /// </summary>
    [SerializeField] private float closeDistance = 0.5f;
    
    private Plant currentPlant = null;

    private AstarSmoothFollow2 follower;
    
    private Transform targetChild;
    private Transform toolParent;
    
    /// <summary>
    /// The tools the player has. Used to spawn the prefabs as children of toolParent
    /// </summary>
    [SerializeField] private List<Tool> tools = new List<Tool>();
    /// <summary>
    /// The animator controller for the tools
    /// </summary>
    private Animator anim;

    private Seeds seed;

    public static AIPlayerBehavior aiPlayer;

    void Start()
    {
        aiPlayer = this;
        toolParent = transform.Find("ToolParent");
        
        follower = GetComponent<AstarSmoothFollow2>();
        anim = GetComponent<Animator>();
        
        if (targetChild == null)
            targetChild = transform.Find("target");
        follower.target = targetChild;
        //Spawn all the player's tools as children under toolParent
        foreach (Tool t in tools)
        {
            GameObject go = Instantiate(t.gameObject, toolParent);
            go.name = t.gameObject.name;
            go.SetActive(false);
        }
        toolParent.GetChild(0).gameObject.SetActive(true);
        seed = toolParent.GetComponentInChildren<Seeds>(includeInactive:true);
        StartCoroutine(checkState());
    }

    void SetTool(string tool)
    {
        foreach (Transform child in toolParent)
        {
            child.gameObject.SetActive(false);
            if (child.name.Contains(tool))
                child.gameObject.SetActive(true);
        }
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
            Plant closest = Plant.findClosestHarvestable(pos);
            if (closest != null)
            {
                SetTool("Hoe");
                follower.target = closest.transform;
                if (_reachedTarget())
                {
                    playAnim("StartHoe");
                    yield return new WaitForSeconds(1.2f);
                    closest.Harvest();
                    follower.target = transform;
                }
                continue;
            }
            closest = Plant.findClosestWaterable(pos);
            if (closest != null)
            {
                SetTool("Can");
                follower.target = closest.transform;
                if (_reachedTarget())
                {
                    playAnim("StartCan");
                    yield return new WaitForSeconds(1.2f);
                    closest.Water();
                }
            }
            else if (FarmTile.UnoccupiedCount() < 3)
            {
                SetTool("Hoe");
                FarmTile ft = FarmTile.findClosestTile(transform.position, false);
                follower.target = ft.transform;
                if (_reachedTarget())
                {
                    playAnim("StartHoe");
                    yield return new WaitForSeconds(1.2f);
                    ft.SetTilled(true);
                }
            }
            else
            {
                SetTool("Seeds");
                FarmTile ft = FarmTile.findClosestTile(transform.position, true);
                follower.target = ft.transform;
                if (_reachedTarget())
                {
                    playAnim("StartSeeds");
                    yield return new WaitForSeconds(1.2f);
                    seed.Use(ft.gameObject);
                }
            }
            
        }
    }
    
    /// <summary>
    /// How close the player is to a given object
    /// </summary>
    /// <param name="target">The GO of the threat to check</param>
    /// <returns>Returns Vector2.distance (ignores Z) of ourself to the target</returns>
    private float distanceToTarget(GameObject target)
    {
        return Vector2.Distance(transform.position, target.transform.position);
    }

    bool _reachedTarget()
    {
        if (distanceToTarget(follower.target.gameObject) <= closeDistance)
        {
            return true;
        }

        return false;
    }

    void playAnim(string animation)
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "None")
        {
            anim.SetTrigger(animation);
        }
        
    }


}
