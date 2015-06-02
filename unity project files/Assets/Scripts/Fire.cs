using UnityEngine;
using System.Collections;

public class Fire : Plant {

	// Use this for initialization
	protected override void Start()
	{
		type = 3;
		base.Start();
		lifeSpan = 480;
	} 
	// Update is called once per frame
	
	protected override void Update()
	{
		byte x = pathFinder.getTerrain(mapX, mapY);
		if (x == 1)
		{
			//If i am in water, i die
			Kill();
		}
		base.Update();
	}
}
