using UnityEngine;
using System.Collections;

public class Volcano : Plant
{
    private bool once = true;
    protected int duration = 0;
    // Use this for initialization
    protected override void Start()
    {
        type = 3;
        base.Start();
    }
    // Update is called once per frame

    protected override void Update()
    {
        duration++;

        if (pathFinder.getTerrain(mapX, mapY) != 6)
        {
            //If i am in water, i die
            Kill();
        }
        base.Update();

        if (once && duration > 180)
        {
            once = false;
            pathFinder.sdv.volcanoStart(mapX, mapY);
        }
    }

    protected virtual void Kill()
    {
        //Talk to spawn
		spawnControl.VolcanoDeath();
        Destroy(this.gameObject);
    }
    
}
