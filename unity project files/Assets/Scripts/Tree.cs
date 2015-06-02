using UnityEngine;
using System.Collections;

public class Tree : Plant{

	// Use this for initialization
    protected override void Start()
    {
        type = 3;
        base.Start();
    } 
	// Update is called once per frame

    protected override void Update()
    {
		byte x = pathFinder.getTerrain(mapX, mapY);
		if (x == 1 || x == 6)
        {
            //If i am in water, i die
            Kill();
        }
        base.Update();
    }
}
