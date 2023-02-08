using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    [SerializeField] private GameObject selectedSprite;

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
    
}
