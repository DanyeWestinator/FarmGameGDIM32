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

    // Update is called once per frame
    void Update()
    {
        
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
    
}
