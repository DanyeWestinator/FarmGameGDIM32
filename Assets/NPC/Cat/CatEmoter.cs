using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// holds emotes sprites as dictionary name -> GO
// displays them by name
public class CatEmoter : MonoBehaviour
{
    // list for inspector access
    public SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> emotes = new List<Sprite>();
    private Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();

    void Start()
    {
        // create dictionary from list
        foreach (Sprite emote in emotes)
        {
            dict.Add(emote.name, emote);
        }
    }
    
    public void display(string name)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = dict[name];
    }

    public void hide()
    {
        spriteRenderer.enabled = false;
    }
}
