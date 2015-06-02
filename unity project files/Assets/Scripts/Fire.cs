using UnityEngine;
using System.Collections;

public class Fire : Plant {

	// Use this for initialization
	protected override void Start()
	{
		type = 3;
		base.Start();
		lifeSpan = 360;
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

	protected override void Kill(){
		if (scaleDownCount <= scaleDownFrames)
		{
			transform.localScale -= new Vector3(0.005f, 0.005f, 0);
			scaleDownCount++;
			return;
		}
		spawnControl.Spawn(-type);
		Destroy(this.gameObject);
		life = 0;
	}
}
