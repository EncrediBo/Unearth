using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Plant : MonoBehaviour {

    public SpawnControl spawnControl;
    public PathFinder pathFinder;

    //Creature type
    /*
     * 1 = fruit
     * 2 = 
     * 
     */
    protected int type = 1;

    //location in game space in pixels
    protected float posX;
    protected float posY;

    //location in the map
    protected int mapX;
    protected int mapY;

    //Age of the plant
    private int life = 0;

    //Map of terrain


    protected const float pixelHeight = 0.023585f;

	// Use this for initialization
	protected virtual void Start () {
	    //Load the position and map position
        LoadPos();

        if (spawnControl == null)
        {
            spawnControl = SpawnControl.Instance();
        }

        if (pathFinder == null)
        {
            pathFinder = PathFinder.Instance();
        }
	}
	
	// Update is called once per frame
	protected virtual void Update () {

        life++;

        if (life > 3000)
        {
            Kill();
        }
	    //This needs to check the land condition to consider if it is dead or not
	}

    protected void LoadPos()
    {
        //Store the initial posistion
        posX = transform.position.x;
        posY = transform.position.y;

        //Convert on cavas position into map position
        mapX = (int)(212 - (posX / pixelHeight));
        mapY = (int)(212 - (posY / pixelHeight));
    }

    protected void OnTriggerEnter2D(Collider2D coll)
    {
       // print("plant got hit");
        Kill();
    }

    protected virtual void Kill()
    {
        //Talk to spawn
        spawnControl.Spawn(-type);
        Destroy(this.gameObject);
        life = 0;
    }

    //Public functions that allow the animal to interact with other elements
    public int getLocationX()
    {
        return mapX;
    }

    public int getLocationY()
    {
        return mapY;
    }
}
