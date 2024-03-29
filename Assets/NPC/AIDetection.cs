using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container for a trigger collider
/// Passes collisions to an AIBehavior
/// maybe check line of sight here and pass it
/// </summary>
public class AIDetection : MonoBehaviour
{
    [SerializeField] AIBehavior behavior;


    private void OnTriggerEnter2D(Collider2D col)
    {
        behavior.OnDetected(col.gameObject);
        // print("detector detected: " + col.gameObject + " by: " + behavior);
    }
}
