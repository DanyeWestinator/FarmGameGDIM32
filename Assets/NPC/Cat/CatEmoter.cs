using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// holds emotes gameobjects as dictionary name -> GO
// displays them by name
// DEPRECIATED - I'M JUST USING A SPRITESHEET IN CAT BEHAVIOR
public class CatEmoter : MonoBehaviour
{
    // list for inspector access
    [SerializeField] private List<GameObject> emotes = new List<GameObject>();
    private Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
    private GameObject activeEmote = null;

    void Start()
    {
        // create dictionary from list
        foreach (GameObject emote in emotes)
        {
            dict.Add(emote.name, emote);
        }
    }

    public void display(string name)
    {
        activeEmote.SetActive(false);
        activeEmote = dict[name];
        activeEmote.SetActive(true);
    }
}
