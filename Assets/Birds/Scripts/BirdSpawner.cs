using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Examples;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdSpawner : MonoBehaviour
{
    public GameObject BirdPrefab;
    
    [SerializeField]
    private float spawnRadius = 10f;
    [SerializeField]
    private float spawnDelay = 4f;
    
    public bool canBirdsSpawn = true;

    private IEnumerator spawnLoop = null;
    // Start is called before the first frame update
    void Start()
    {
        StartLoop();
    }

    private void OnEnable()
    {
        StartLoop();
    }

    void StartLoop()
    {
        if (spawnLoop != null)
            return;
        spawnLoop = birdSpawnLoop();
        StartCoroutine(spawnLoop);
    }
    IEnumerator birdSpawnLoop()
    {
        while (true)
        {
            if (canBirdsSpawn)
                SpawnBird();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnBird()
    {
        Vector3 pos = Random.insideUnitCircle * spawnRadius;
        pos.z = -1;
        Plant p = FarmSpawner.findClosestPlant(pos);
        if (p == null)
        {
            return;
        }
        GameObject spawned = Instantiate(BirdPrefab, transform);
        spawned.transform.position = pos;
        //spawned.GetComponent<AstarSmoothFollow2>().target = PlayerController.player.transform;
        //print("Bird spawned!");
    }
}
