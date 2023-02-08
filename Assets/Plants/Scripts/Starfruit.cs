using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfruit : Plant
{
    protected override void Awake()
    {
        base.Awake();
        growthStageLength = 15.0f; // update the length of each growth stage for Starfruit plants
        growthStagesTotal = 8; // update the total number of growth stages for Starfruit plants
    }
}
