using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for AI behavior (called by detection, ) 
/// Place on NPC base (uses position etc)
/// </summary>
public class AIBehavior: MonoBehaviour
{
    public IMovingAI AIMover; 
    public virtual void OnDetected(GameObject gameObject)
    {
        
    }
}
