using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lives on the isTrigger collider, slightly offset from the player.
/// <br />Detects where the player is currently looking
/// </summary>
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
        Vector2Int pos = Vector2Int.RoundToInt(transform.position);
        //Updates the tile the player is on
        //Dict used for O(1) lookups
        if (FarmSpawner.tiles.ContainsKey(pos))
            player.OnHit(FarmSpawner.tiles[pos]);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //player.OnHit(col.gameObject);
    }
}
