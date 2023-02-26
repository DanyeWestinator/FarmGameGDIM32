using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// holds emotes sprites as dictionary name -> GO
// displays them by name
public class CatEmoter : MonoBehaviour
{
    // list for inspector access
    [SerializeField] private List<SpriteRenderer> emotes = new List<SpriteRenderer>();
    private Dictionary<string, SpriteRenderer> dict = new Dictionary<string, SpriteRenderer>();
    private SpriteRenderer activeEmote = null;

    void Start()
    {
        // create dictionary from list
        foreach (SpriteRenderer emote in emotes)
        {
            dict.Add(emote.name, emote);
        }
    }
    
    public void display(string name)
    {
        if (activeEmote) activeEmote.enabled = false;
        activeEmote = dict[name];
        activeEmote.enabled = true;
    }
}
