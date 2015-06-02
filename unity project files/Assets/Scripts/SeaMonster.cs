using UnityEngine;
using System.Collections;

public class SeaMonster : Plant
{
    // Use this for initialization
    protected override void Start()
    {
        type = 3;
        base.Start();
    }
    // Update is called once per frame

    protected override void Update()
    {

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

