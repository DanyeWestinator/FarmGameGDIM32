using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private Color[] colors = { Color.blue, Color.green, Color.yellow }; // TEMP

    private float wateredTime; // the time this plant was watered

    private int growthStage = 1; // the current growth stage
    private int growthStagesTotal = 4; // the total number of growth stages for this plant
    private float growthStageLength = 5.0f; // how long each growth stage lasts in seconds

    private bool harvestable = false; // true when ready to harvest
    private bool watered = false; // true if has been watered during the plant's current growth stage

    private GameObject needsWaterIcon;
    private GameObject harvestableIcon;

    void Awake()
    {
        needsWaterIcon = gameObject.transform.GetChild(0).gameObject;
        
        harvestableIcon = gameObject.transform.GetChild(1).gameObject;
        harvestableIcon.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        //if (!harvestable && Time.time >= plantedTime + (growthStage * growthStageLength))
        if (watered && !harvestable && Time.time >= wateredTime + growthStageLength)
        {
            Grow();
        }
    }

    // The plant grows to the next stage in its growth cycle
    void Grow()
    {
        GetComponent<Renderer>().material.color = colors[growthStage - 1]; // TEMP
        growthStage++;

        if (growthStage == growthStagesTotal)
        { 
            harvestable = true;
            harvestableIcon.SetActive(true);
            //Debug.Log("ready to harvest!"); // DEBUG
        }
        else
        {
            watered = false;
            needsWaterIcon.SetActive(true);
        }
    }

    public void Water()
    {
        if (!watered)
        {
            Debug.Log("~watered~"); // DEBUG
            watered = true;
            needsWaterIcon.SetActive(false);
            wateredTime = Time.time;
        }
    }

    public void Harvest()
    {
        if (harvestable)
        {
            Debug.Log("~harvested~"); // DEBUG
            gameObject.SetActive(false);
        }
    }
}
