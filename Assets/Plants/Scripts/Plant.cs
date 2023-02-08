using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private Sprite[] growthSprites;

    private float wateredTime; // the time this plant was watered

    private int growthStage = 1;
    [SerializeField]// the current growth stage
    protected int growthStagesTotal = 5;
    [SerializeField]// the total number of growth stages for this plant
    protected float growthStageLength = 3.0f; // how long each growth stage lasts in seconds
    [SerializeField]
    protected int totalHarvests = 1; // total number of harvests this plant has
    private int harvestNum = 0; // current harvest number

    private bool harvestable = false; // true when ready to harvest
    private bool watered = false; // true if has been watered during the plant's current growth stage

    private SpriteRenderer spriteRenderer;
    private GameObject needsWaterIcon;
    private GameObject harvestableIcon;

    [SerializeField] private int scoreValue = 1;

    protected virtual void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = growthSprites[0];
        
        needsWaterIcon = gameObject.transform.GetChild(0).gameObject;
        
        harvestableIcon = gameObject.transform.GetChild(1).gameObject;
        harvestableIcon.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (watered && !harvestable && Time.time >= wateredTime + growthStageLength)
        {
            Grow();
        }
    }

    // The plant grows to the next stage in its growth cycle
    private void Grow()
    {
        
        growthStage++;
        if (growthStage < growthSprites.Length)
            spriteRenderer.sprite = growthSprites[growthStage-1];

        if (growthStage == growthStagesTotal || growthStage == growthSprites.Length)
        { 
            harvestable = true;
            harvestableIcon.SetActive(true);
        }
        else
        {
            watered = false;
            needsWaterIcon.SetActive(true);
        }
    }

    // Called when the watering can tool is used on the plant, waters the plant for the current growth stage
    public void Water()
    {
        if (!watered)
        {
            //Debug.Log("~watered~"); // DEBUG
            watered = true;
            needsWaterIcon.SetActive(false);
            wateredTime = Time.time;
        }
    }

    // Called when the scythe tool is used on the plant, only harvests the plant if is at final growth stage
    // If plant was successfully harvested, the player's score increases
    public void Harvest()
    {
        if (harvestable)
        {
            harvestNum++;
            
            // TEMP add to score variable HERE !!!!!!

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
            PlayerController.player.AddScore(scoreValue);
        }
    }

    // Called when the shovel tool is used on the plant, destroys the plant
    public void Dig()
    {
        //Debug.Log("~dig up~"); // DEBUG
        Destroy(gameObject);
    }
}
