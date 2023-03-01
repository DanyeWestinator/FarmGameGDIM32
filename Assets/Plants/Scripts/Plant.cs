using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private Sprite[] growthSprites;

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
    private bool watered = false; // true if has been watered during the plant's current growth stage
    private bool wilted = false; // true if the plant has wilted and died

    private SpriteRenderer spriteRenderer;
    private GameObject needsWaterIcon;
    private GameObject harvestableIcon;

    [SerializeField] protected int costValue = 0;
    [SerializeField] protected int scoreValue = 1;

    protected virtual void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = growthSprites[0];

        needsWaterIcon = gameObject.transform.GetChild(0).gameObject;
        needsWaterIcon.SetActive(false);
        growthTime = Time.time;

        harvestableIcon = gameObject.transform.GetChild(1).gameObject;
        harvestableIcon.SetActive(false);

        currentStageLength = Random.Range(minStageLength, maxStageLength);

        // Pay the cost of the seed from the player's score
        PlayerController.player.AddScore(-costValue);
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
                    needsWaterIcon.SetActive(true);
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
        currentStageLength = Random.Range(minStageLength, maxStageLength);

        if (growthStage <= growthSprites.Length)
            spriteRenderer.sprite = growthSprites[growthStage - 1];

        if (growthStage == growthStagesTotal || growthStage == growthSprites.Length)
        {
            harvestable = true;
            harvestableIcon.SetActive(true);
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
            watered = true;
            canWater = false;
            needsWaterIcon.SetActive(false);
            wateredTime = Time.time;
        }
    }

    // Called when the scythe tool is used on the plant, only harvests the plant if is at final growth stage
    // If plant was successfully harvested, the player's score increases
    public void Harvest()
    {
        if (harvestable && !wilted)
        {
            harvestNum++;

            // Add to player's score
            PlayerController.player.AddScore(scoreValue);

            if (harvestNum == totalHarvests)
            {
                Destroy(gameObject);
            }
            else
            {
                growthStage -= 3;
                harvestable = false;
                harvestableIcon.SetActive(false);
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
        wilted = true;
        spriteRenderer.sprite = growthSprites[growthStagesTotal];
        harvestableIcon.SetActive(false);
        needsWaterIcon.SetActive(false);
    }

}
