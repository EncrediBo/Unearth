using UnityEngine;
using System.Collections;

public class SeaMonster : Plant
{
	private int myAngle = 0;

    // Use this for initialization
    protected override void Start()
    {
        type = 3;
        base.Start();
    }
    // Update is called once per frame

    protected override void Update()
    {
		myAngle += 5;
		transform.rotation = Quaternion.Euler(0, 0, -myAngle);
        if (pathFinder.getTerrain(mapX, mapY) != 7)
        {
            //If i am in water, i die
            Kill();
        }


    }

    protected virtual void Kill()
    {
        //Talk to spawn
        spawnControl.seaMonsterSpawn = true;
        transform.position += new Vector3(pixelHeight, -pixelHeight, 0f);
        Destroy(this.gameObject);
    }
    
}

