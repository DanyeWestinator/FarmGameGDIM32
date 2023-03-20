using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// quick script to spawn the AI player
public class AIPlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;

    public void Spawn()
    {
        print("spawning!");
        Instantiate(PlayerPrefab);
    }
}
