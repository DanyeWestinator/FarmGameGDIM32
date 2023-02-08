using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : Plant
{
    protected override void Awake()
    {
        base.Awake();
        //growthStageLength = 6.0f; // update the length of each growth stage for Potato plants
        //growthStagesTotal = 6; // update the total number of growth stages for Potato plants
    }
}
