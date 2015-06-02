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
    protected int life = 0;
    protected int lifeSpan;

	//counter for spawn animation
	private int scaleCount = 0;
	private int scaleFrames = 100;

	//counter for death animation
	protected int scaleDownCount = 0;
	protected int scaleDownFrames = 65;

    //Map of terrain


    protected const float pixelHeight = 0.023585f;

	// Use this for initialization
	protected virtual void Start () {
        //Generate a random life span
        lifeSpan = Random.Range(3000, 4000);

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

		//set initial scale to zero
		Vector3 scale = new Vector3( 0, 0, 1f );
		//transform.localScale = scale;
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {

        life++;

        if (life > lifeSpan)
        {
            //decrease size animation
            /*if (scaleDownCount <= scaleDownFrames)
            {
                transform.localScale -= new Vector3(0.005f, 0.005f, 0);
                scaleDownCount++;
                return;
            } */
            Kill(); 
        }
            //This needs to check the land condition to consider if it is dead or not

        //spawning animation
        if (scaleCount <= scaleFrames)
        {
            transform.localScale += new Vector3(0.01F, 0.01F, 0);
            scaleCount++;
        }
    
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
        //Kill();
    }

    protected virtual void Kill()
    {
        //Talk to spawn
        if (scaleDownCount <= scaleDownFrames)
        {
            transform.localScale -= new Vector3(0.005f, 0.005f, 0);
            scaleDownCount++;
            return;
        }

		//Check to see if fire will be lite
		if (pathFinder.getTerrain (mapX, mapY) == 6) {
			spawnControl.LightFire(mapX, mapY);
		}
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
