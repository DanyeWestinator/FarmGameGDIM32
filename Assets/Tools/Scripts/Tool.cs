using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base tool class
/// </summary>
public abstract class Tool : MonoBehaviour
{

    public AudioSource audioSource; 
   
    /// <summary>
    /// Public facing logic to use a tool. Handles generic tool logic,
    /// then each subclass must implement their own logic
    /// </summary>
    public void UseTool(GameObject tile)
    {
        //Will play animation, check hitboxes and find interactable, etc
        //print($"Using {gameObject.name}! Hopefully, logic to follow!");
        if (audioSource != null) audioSource.Play();
        Use(tile);
    }
    /// <summary>
    /// Each subclass must override
    /// </summary>
    public abstract void Use(GameObject tile);
    
}
