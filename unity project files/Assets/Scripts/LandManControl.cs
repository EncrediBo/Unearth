using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandManControl : Animal
{

    // Use this for initialization
    protected override void Update()
    {
        //Choose which heat map as path finding guide
        heatMap = pathFinder.getHeatMap(1);

        base.Update();
    }

}
