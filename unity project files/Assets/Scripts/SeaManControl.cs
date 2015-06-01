using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeaManControl : Animal
{
    // Use this for initialization
    protected override void Start()
    {
        type = 2;
        terrain = 1;
        base.Start();
    }
    // Use this for initialization
    protected override void Update()
    {
        if (myStateDuration >= 90)
        {
            ChangeState(State.Seeking);
			// change the animator here to the idle animation
			base.animator.SetTrigger("idle");
        }


        base.Update();
    }

    protected override void CheckTerrain()
    {
        //Spawning code
        if (terrainMap[mapY * 424 + mapX] != 1)//|| heatMap[mapY * 424 + mapX] == 0)
        {
            //The animal is currently in a terrain that it cannot move in
            ChangeState(State.Drowning);

			//change the animator here to drowning animation
			base.animator.SetTrigger("drowning");
        }
    }
}
