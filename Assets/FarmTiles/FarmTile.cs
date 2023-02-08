using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    [SerializeField] private GameObject selectedSprite;
    // Start is called before the first frame update
    void Start()
    {
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
