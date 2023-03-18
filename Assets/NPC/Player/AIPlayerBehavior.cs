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
    
    private Plant currentPlant = null;

    private AstarSmoothFollow2 follower;
    
    private Transform targetChild;


    void Start()
    {
        follower = GetComponent<AstarSmoothFollow2>();
        
        if (targetChild == null)
            targetChild = transform.Find("target");
        follower.target = targetChild;
        StartCoroutine(checkState());

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


}
