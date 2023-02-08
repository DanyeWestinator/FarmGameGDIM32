using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : Plant
{
    protected override void Awake()
    {
        base.Awake();
        growthStageLength = 13.0f; // update the length of each growth stage for Melon plants
        growthStagesTotal = 7; // update the total number of growth stages for Melon plants
    }
}
