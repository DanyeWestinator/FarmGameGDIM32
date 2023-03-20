using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each grid space has a farm tile, farm tiles can have things in them
/// </summary>
public class FarmTile : MonoBehaviour
{
    /// <summary>
    /// The sprite that turns on/off when the player is in a square
    /// </summary>
    [SerializeField] private GameObject selectedSprite;
    /// <summary>
    /// The current state of the soil
    /// </summary>
    [SerializeField] private SpriteRenderer tilledSpriteRenderer;
    [SerializeField]
    private Sprite untilledSprite;
    [SerializeField]
    private Sprite tilledSprite;

    public bool tilled = false;
    
    public GameObject occupiedBy;
    // Start is called before the first frame update
    void Start()
    {
        //Add self to dict of tiles by pos
        Vector2Int pos = Vector2Int.RoundToInt(transform.position);
        if (FarmSpawner.tiles.ContainsKey(pos) == false)
            FarmSpawner.tiles.Add(pos, gameObject);
    }

    public void SetSelected(bool set)
    {
        selectedSprite.SetActive(set);
    }

    public void SetTilled(bool set)
    {
        tilled = set;
        if (tilled)
        {
            tilledSpriteRenderer.sprite = tilledSprite;
        }
        else
        {
            tilledSpriteRenderer.sprite = untilledSprite;
        }
            
    }
    /// <summary>
    /// Finds the closest FarmTile to any given position
    /// </summary>
    /// <param name="pos">The position to find the closest farm tile to</param>
    /// <param name="tilled">Whether to be searching for the closest tilled or untilled</param>
    /// <returns></returns>
    public static FarmTile findClosestTile(Vector2 pos, bool tilled = true)
    {
        Vector2Int posint = Vector2Int.RoundToInt(pos);
        FarmTile closest = null;
        float distance = float.MaxValue;
        foreach (Vector2Int tile in FarmSpawner.tiles.Keys)
        {
            //If this tile is closer than any other
            float d = Vector2Int.Distance(tile, posint);
            if (d < distance)
            {
                FarmTile ft = FarmSpawner.tiles[tile].GetComponent<FarmTile>();
                //Tilled and supposed to be, or not tilled and not supposed to be
                if ((ft.tilled == tilled) && ft.occupiedBy == null)
                {
                    closest = ft;
                    distance = d;
                }
            }
        }

        return closest;
    }
    /// <summary>
    /// How many tiles have been tilled, but no plants yet
    /// </summary>
    /// <returns></returns>
    public static int UnoccupiedCount()
    {
        int count = 0;
        foreach (GameObject go in FarmSpawner.tiles.Values)
        {
            FarmTile ft = go.GetComponent<FarmTile>();
            if (ft.occupiedBy == null && ft.tilled)
                count++;
        }
        return count;
    }
    
}
