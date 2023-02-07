using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyes : MonoBehaviour
{
    /// <summary>
    /// This is the player game object
    /// <br /> line break
    /// </summary>
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = Vector3Int.RoundToInt(transform.position);
        player.OnHit(FarmSpawner.tiles[pos]);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //player.OnHit(col.gameObject);
    }
}
