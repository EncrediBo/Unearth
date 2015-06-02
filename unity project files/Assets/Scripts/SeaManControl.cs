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
		delay = 0.05f;
        base.Start();
    }
    // Use this for initialization
    protected override void Update()
    {
		//Choose which heat map as path finding guide
		heatMap = pathFinder.getHeatMap(type);
        
		if (heatMap [mapY * 424 + mapX] > 9950) {
			ChangeState(State.Hunting);
			base.animator.SetBool("drowning", false);
		}
        else if (myState != State.Seeking)
        {
			//If this have not been move for more than 90 frames start seeking
            ChangeState(State.Seeking);
			// change the animator here to the idle animation
            //Debug.Log("state should be seeking but is: " + myState);
			base.animator.SetBool("drowning", false);
        }


        base.Update();
    }

    protected override void CheckTerrain()
    {
        //Spawning code
        if (terrainMap[mapY * 424 + mapX] != 1 && terrainMap[mapY * 424 + mapX] != 7)//|| heatMap[mapY * 424 + mapX] == 0)
        {
            //The animal is currently in a terrain that it cannot move in
            ChangeState(State.Drowning);
            //Debug.Log("State should be drowning but is: " + myState);
			//change the animator here to drowning animation
            base.animator.SetBool("drowning", true);
        }
    }
}
