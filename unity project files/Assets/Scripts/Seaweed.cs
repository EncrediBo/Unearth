using UnityEngine;
using System.Collections;

public class Seaweed : Plant {

	// Use this for initialization
    protected override void Start()
    {
        type = 4;
        base.Start();
    } 
	// Update is called once per frame
    protected override void Update()
    {
        if (pathFinder.getTerrain(mapX, mapY) >= 2)
        {
            //If i am NOT in water, i die
            Kill();
        }
 	     base.Update();
    }
}
