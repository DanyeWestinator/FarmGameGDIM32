using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Plant
{
    protected override void Awake()
    {
        base.Awake();
        //growthStageLength = 7.0f; // update the length of each growth stage for Tomato plants
        //growthStagesTotal = 7; // update the total number of growth stages for Tomato plants
        //totalHarvests = 3; // update the total number of harvests for Tomato plants so they can be harvested multiple times
    }
}
