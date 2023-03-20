using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using Random = UnityEngine.Random;

public class Plant : MonoBehaviour
{
    [SerializeField] protected Sprite[] growthSprites;
    [SerializeField] protected Sprite[] emoteSprites;

    private float wateredTime; // the time this plant was watered
    private float growthTime; // the time this plant started its current growth stage at

    private int growthStage = 1; // the current growth stage
    [SerializeField] protected int growthStagesTotal = 5; // the total number of growth stages for this plant
    [SerializeField] protected float minStageLength = 3.0f; // min length of each growth stage in seconds
    [SerializeField] protected float maxStageLength = 4.0f; // max length of each growth stage in seconds
    private float currentStageLength; // each growth stage, the currentStageLength is set to a new random number between the minStageLength & maxStageLength

    [SerializeField] protected int totalHarvests = 1; // total number of harvests this plant has
    private int harvestNum = 0; // current harvest number

    private float timeUntilWater = 1.5f; // after plant grows 1 stage, must wait this amount of seconds before can water it
    [SerializeField] protected float timeUntilWilts = 30.0f; // if not watered within this amount of seconds, the plant will wilt and die
    
    private bool canWater = false; // true when plant can be watered
    private bool harvestable = false; // true when ready to harvest
    /// <summary>
    /// Public getter to see if a plant can be harvested
    /// </summary>
    public bool Harvestable => harvestable;
    private bool watered = false; // true if has been watered during the plant's current growth stage
    /// <summary>
    /// Public getter for if the plant is dead
    /// </summary>
    public bool IsDead => wilted;
    private bool wilted = false; // true if the plant has wilted and died
    private bool waterWarning1 = false; // true if the first water warning has been given
    [SerializeField] private float waterWarning1SecRemaining = 15f; // wilt warning 1 appears when there's this many seconds left until plant wilts
    private bool waterWarning2 = false; // true if the second water warning has been given
    [SerializeField] private float waterWarning2SecRemaining = 8f; // wilt warning 2 appears when there's this many seconds left until plant wilts

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer emoteRenderer;
    private GameObject emoteIcon;

    [SerializeField] protected int costValue = 0; // how much it costs to plant this plant
    [SerializeField] protected int scoreValue = 1; // how much the player receives each time this plant is harvested successfully

    protected Dictionary<string, Sprite> emoteDictionary; // stores the emotes from emoteSprites in this dict at easily accessible string keys

    private static HashSet<Plant> allPlants = new HashSet<Plant>();

    protected virtual void Start()
    {
        emoteDictionary = new Dictionary<string, Sprite>();

        emoteDictionary.Add("water", emoteSprites[0]);
        emoteDictionary.Add("water yellow", emoteSprites[1]);
        emoteDictionary.Add("water red", emoteSprites[2]);

        emoteDictionary.Add("bird", emoteSprites[3]);
        emoteDictionary.Add("bird yellow", emoteSprites[4]);
        emoteDictionary.Add("bird red", emoteSprites[5]);

        emoteDictionary.Add("dead", emoteSprites[6]);
        emoteDictionary.Add("harvest", emoteSprites[7]);
    }

    protected virtual void Awake()
    {
        growthTime = Time.time;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = growthSprites[0];

        emoteIcon = gameObject.transform.GetChild(0).gameObject;
        emoteIcon.SetActive(false);
        emoteRenderer = emoteIcon.GetComponent<SpriteRenderer>();

        currentStageLength = Random.Range(minStageLength, maxStageLength); // randomizes the length of the current growth stage

        // Pay the cost of the seed from the player's score
        if (PlayerController.player != null)
            PlayerController.player.AddScore(-costValue);
    }

    private void OnEnable()
    {
        allPlants.Add(this);
    }

    void OnDisable()
    {
        allPlants.Remove(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!wilted)
        {
            if (!watered)
            {
                // after a plant just grew a stage, must wait timeUntilWater seconds before it can be watered
                if (!canWater && Time.time >= growthTime + timeUntilWater)
                {
                    canWater = true;
                    Emote("water");
                }

                // 7 seconds until wilts water warning
                if (!waterWarning1 && Time.time >= growthTime + timeUntilWilts - waterWarning1SecRemaining)
                {
                    waterWarning1 = true;
                    Emote("water yellow");
                }

                // 3 seconds until wilts water warning
                if (!waterWarning2 && Time.time >= growthTime + timeUntilWilts - waterWarning2SecRemaining)
                {
                    waterWarning2 = true;
                    Emote("water red");
                }

                // if plant is not watered within timeUntilWilts seconds of starting the current growth stage, it wilts and dies
                if (Time.time >= growthTime + timeUntilWilts)
                {
                    Die();
                }
            }
            // if plant is watered, not harvestable, and currentStageLength seconds have passed, it grows to the next growth stage
            else if (!harvestable && Time.time >= wateredTime + currentStageLength)
            {
                Grow();
            }
        }
    }

    // The plant grows to the next stage in its growth cycle
    private void Grow()
    {
        growthTime = Time.time;

        growthStage++;
        currentStageLength = Random.Range(minStageLength, maxStageLength); // randomizes the length of the current growth stage

        if (growthStage <= growthSprites.Length-1)
            spriteRenderer.sprite = growthSprites[growthStage - 1];

        if (growthStage == growthStagesTotal || growthStage == growthSprites.Length-1)
        {
            harvestable = true;
            Emote("harvest");
        }
        else
        {
            watered = false;
        }
    }

    // Called when the watering can tool is used on the plant, waters the plant for the current growth stage
    public void Water()
    {
        if (!watered && canWater && !wilted)
        {
            wateredTime = Time.time;

            watered = true;
            canWater = false;
            waterWarning1 = false;
            waterWarning2 = false;

            emoteIcon.SetActive(false);
        }
    }

    // Called when the harvesting tool is used on the plant, only harvests the plant if is at final growth stage
    // If plant was successfully harvested, the player's score increases
    public void Harvest()
    {
        if (harvestable && !wilted)
        {
            harvestNum++;

            // Add to player's score
            ScoreKeeper.instance.AddScore(scoreValue);
            //PlayerController.player.AddScore(scoreValue);

            if (harvestNum == totalHarvests)
            {
                Destroy(gameObject);
            }
            else
            {
                harvestable = false;
                emoteIcon.SetActive(false);

                growthStage -= 3;
                Grow();
            }
        }
    }

    // Called when the shovel tool is used on the plant, destroys the plant
    public void Dig()
    {
        Destroy(gameObject);
    }

    // Called when the plant dies and wilts
    public void Die()
    {
        spriteRenderer.sprite = growthSprites[growthStagesTotal];
        wilted = true;
        harvestable = false;
        canWater = false;
        waterWarning1 = false;
        waterWarning2 = false;
        Emote("dead");
    }

    // Changes the currently displayed emote (if any) with the given emote
    public void Emote(string emote)
    {
        emoteRenderer.sprite = emoteDictionary[emote];
        emoteIcon.SetActive(true);
    }

    // Removes the currently displayed emote and replaces it with an emote depicting a currently true condition (if any)
    // if there's no conditions that require an emote right now, then it turns off the emote
    // Mainly intended to be used by outside classes that are affecting the plant's emotes
    public void RemoveEmote()
    {
        if (wilted)
        {
            Emote("dead");
        }
        else if (harvestable)
        {
            Emote("harvest");
        }
        else if (waterWarning2)
        {
            Emote("water red");
        }
        else if (waterWarning1)
        {
            Emote("water yellow");
        }
        else if (canWater)
        {
            Emote("water");
        }
        else
        {
            emoteIcon.SetActive(false);
        }
    }
    /// <summary>
    /// Finds the closest plant that is able to be harvested, excludes catnip
    /// </summary>
    /// <param name="pos">The position to find closest to</param>
    /// <returns></returns>
    public static Plant findClosestHarvestable(Vector2 pos)
    {
        Plant closest = null;
        float dis = float.MaxValue;

        foreach (Plant p in allPlants)
        {
            //Ignore all non-applicable plants
            if (p.harvestable == false || p.IsDead || p.GetComponent<Catnip>())
                continue;
            float d = Vector2.Distance(p.transform.position, pos);
            if (d < dis)
            {
                dis = d;
                closest = p;
            }
        }
        return closest;
    }

    /// <summary>
    /// Finds the closest harvestable catnip (used by the cat)
    /// </summary>
    /// <param name="pos">The position to find closest to</param>
    /// <returns></returns>
    public static Plant findClosestCatnip(Vector2 pos)
    {
        Plant closest = null;
        float dis = float.MaxValue;

        foreach (Plant p in allPlants)
        {
            //Ignore all non-applicable plants
            if (p.harvestable == false || p.IsDead || !p.GetComponent<Catnip>())
                continue;
            float d = Vector2.Distance(p.transform.position, pos);
            if (d < dis)
            {
                dis = d;
                closest = p;
            }
        }
        return closest;
    }


    /// <summary>
    /// Finds the closest plant that is able to be watered
    /// </summary>
    /// <param name="pos">The position to find closest to</param>
    /// <returns></returns>
    public static Plant findClosestWaterable(Vector2 pos)
    {
        Plant closest = null;
        float dis = float.MaxValue;
        foreach (Plant p in allPlants)
        {
            //Ignore all non Waterable
            if (p.canWater == false || p.IsDead)
                continue;
            float d = Vector2.Distance(p.transform.position, pos);
            if (d < dis)
            {
                dis = d;
                closest = p;
            }
        }
        return closest;
    }

}
