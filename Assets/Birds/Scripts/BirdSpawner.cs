using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject BirdPrefab;
    
    [SerializeField]
    private float spawnRadius = 10f;
    [SerializeField]
    private float spawnDelay = 4f;
    
    public bool canBirdsSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(birdSpawnLoop());
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP
        if (Input.GetButtonDown("Fire1") && false)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            Instantiate(BirdPrefab, pos, Quaternion.identity);
        }
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
        GameObject spawned = Instantiate(BirdPrefab, transform);
        spawned.transform.position = pos;

    }
}
