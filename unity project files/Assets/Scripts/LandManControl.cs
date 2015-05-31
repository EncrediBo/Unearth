using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandManControl : Animal
{

    // Use this for initialization
    protected override void Start()
    {
        type = 1;
        terrain = 3;
        base.Start();
    }
    protected override void Update()
    {
        if (myStateDuration >= 90)
        {
            ChangeState(State.Hunting);
        }

        //Get terrain map
        terrainMap = pathFinder.sdv.getMap();

        base.Update();
    }

}
