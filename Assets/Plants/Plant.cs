using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private GameObject selectedSprite;
    public GameObject occupied;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelected(bool set)
    {
        selectedSprite.SetActive(set);
    }

    public void OnUse(string tool)
    {
        //Do nothing if tile empty
        if (occupied == null)
            return;
        //If can be chopppied
        if (tool == "Axe" && occupied.gameObject.name.ToLower().Contains("tree"))
        {
            Destroy(occupied.gameObject);
        }
    }
}
