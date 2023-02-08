using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat : Plant
{
    protected override void Awake()
    {
        base.Awake();
        //growthStageLength = 5.0f; // update the length of each growth stage for Wheat plants
    }
}
