using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject BirdPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP
        // if (Input.GetButtonDown("Fire1"))
        // {
        //     var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     pos.z = 0;
        //     Instantiate(BirdPrefab, pos, Quaternion.identity);
        // }
    }
}
