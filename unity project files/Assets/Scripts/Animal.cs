using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Animal : MonoBehaviour
{

    public PathFinder pathFinder;
    public SpawnControl spawnControl;

    //public static int count = 1;

    //Creature type
    /*
     * 1 = landMan
     * 2 = seaMan
     * 
     */
    protected int type = 1;

    //location in game space in unity units
    protected float posX;
    protected float posY;

    //location in the map
    protected int mapX;
    protected int mapY;

    //Direction of this sprite is facing
    private Direction facing;

    protected State myState = State.Idle;
    protected int myStateDuration = 0;

    //Movement speed delay
    //Time interval for recalculation
    private float delay = 0.02f; //Every two seconds
    private float timer = 0f;

    protected int[] heatMap;

    protected const float pixelHeight = 0.023585f;
 
    // Use this for initialization
    protected virtual void Start()
    {
        //Load the position and map position
        LoadPos();

        if (pathFinder == null)
        {
            pathFinder = PathFinder.Instance();
        }

        if (spawnControl == null)
        {
            spawnControl = SpawnControl.Instance();
        }
        facing = Direction.DONTMOVE;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Choose which heat map as path finding guide
        heatMap = pathFinder.getHeatMap(type);

        //Applying position, just in case it is bumped
        LoadPos();

        //Spawning code
        if (heatMap[mapY * 424 + mapX] == -1)//|| heatMap[mapY * 424 + mapX] == 0)
        {
            //The animal is currently in a terrain that it cannot move in

            return;
        }

        if (heatMap[mapY * 424 + mapX] == 10000)
        {
            //spawnControl.Spawn(type);
            Kill();
            //spawnControl.Spawn(-(type));

        }

        if(myStateDuration >= 120) {
            ChangeState(State.Eating);
        }

        switch (myState){
            case State.Idle:
                myStateDuration++;
                break;
        }

        //moves character using current position and the heatmap choosen
        Move(checkHeat(mapX, mapY, heatMap));
    }

    protected void ChangeState(State state)
    {
        myStateDuration = 0;
        myState = state;
    }

    public enum Direction
    {
        North,
        East,
        South,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest,
        DONTMOVE
    }

    public enum State
    {
        Eating,
        Seeking,
        Idle,
        Hunting,
        Hurting,
        Drowning,
        Dying,
        Dance
    }

    void OnTriggerEnter2D(Collider2D coll){

        print("animal got hit");
        if(coll.tag.Equals("Food")){
            print("nom nom nom");
            ChangeState(State.Eating);
        } else {
            print("Ouch");
        }
        print(coll);
        //coll.gameObject.SetActive(false);
    }

    protected Direction checkHeat (int x, int y, int[] heatMap)
    {
        int curr = heatMap[y * 424 + x];
        int newHeight;
        Direction myDirection = Direction.DONTMOVE;
        //int rnd = Random.Range(0, 10);

        //Determine which direction this is heading according to heat and current direction
        if ((x - 1) > 0)
        {
            newHeight = heatMap[y * 424 + (x - 1)];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.East;
            }
            else if (newHeight == curr && facing == Direction.East)
            {
                curr = newHeight;
                myDirection = Direction.East;
            }
        }

        if ((x + 1) < 423)
        {
            newHeight = heatMap[y * 424 + (x + 1)];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.West;
            }
            else if (newHeight == curr && facing == Direction.West)
            {
                curr = newHeight;
                myDirection = Direction.West;
            }
        }

        if ((y - 1) > 0)
        {
            newHeight = heatMap[(y - 1) * 424 + x];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.North;
            }
            else if (newHeight == curr && facing == Direction.North)
            {
                curr = newHeight;
                myDirection = Direction.North;
            }
        }

        if ((x - 1) > 0 && (y - 1) > 0)
        {
            newHeight = heatMap[(y - 1) * 424 + x - 1];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.NorthEast;
            }
            else if (newHeight == curr && facing == Direction.NorthEast)
            {
                curr = newHeight;
                myDirection = Direction.NorthEast;
            }
        }

        if ((x + 1) < 423 && (y - 1) > 0)
        {
            newHeight = heatMap[(y - 1) * 424 + x + 1];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.NorthWest;
            }
            else if (newHeight == curr && facing == Direction.NorthWest)
            {
                curr = newHeight;
                myDirection = Direction.NorthWest;
            }
        }

        if ((y + 1) < 423)
        {
            newHeight = heatMap[(y + 1) * 424 + x];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.South;
            }
            else if (newHeight == curr && facing == Direction.South)
            {
                curr = newHeight;
                myDirection = Direction.South;
            }
        }

        if ((x + 1) < 423 && (y + 1) < 423)
        {
            newHeight = heatMap[(y + 1) * 424 + x + 1];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.SouthWest;
            }
            else if (newHeight == curr && facing == Direction.SouthWest)
            {
                curr = newHeight;
                myDirection = Direction.SouthWest;
            }
        }

        if ((x - 1) > 0 && (y + 1) < 423)
        {
            newHeight = heatMap[(y + 1) * 424 + x - 1];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.SouthEast;
            }
            else if (newHeight == curr && facing == Direction.SouthEast)
            {
                curr = newHeight;
                myDirection = Direction.SouthEast;
            }
        }

        //Final direction is returned
        return myDirection;
    }


    protected void Move(Direction dir)
    {
        //Debug.Log(i);
        //Movement delay
        timer += Time.deltaTime;
        if (timer > delay)
        {
            timer = 0f;
        }
        else
        {
            return;
        }

        switch (dir)
        {
            case Direction.NorthWest:
                mapX++;
                mapY--;
                transform.rotation = Quaternion.Euler(0, 0, 45);
                transform.position += new Vector3(-pixelHeight, pixelHeight, 0f);//up left
                break;
            case Direction.North:
                mapY--;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.position += new Vector3(0f, pixelHeight, 0f);//up
                break;
            case Direction.NorthEast:
                mapX--;
                mapY--;
                transform.rotation = Quaternion.Euler(0, 0, 315);
                transform.position += new Vector3(pixelHeight, pixelHeight, 0f);//up right
                break;
            case Direction.West:
                mapX++;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                transform.position += new Vector3(-pixelHeight, 0f, 0f);//left
                break;
            case Direction.East:
                mapX--;
                transform.rotation = Quaternion.Euler(0, 0, 270);
                transform.position += new Vector3(pixelHeight, 0f, 0f);//right
                break;
            case Direction.SouthWest:
                mapX++;
                mapY++;
                transform.rotation = Quaternion.Euler(0, 0, 135);
                transform.position += new Vector3(-pixelHeight, -pixelHeight, 0f);//down left
                break;
            case Direction.South:
                mapY++;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                transform.position += new Vector3(0f, -pixelHeight, 0f);//down
                break;
            case Direction.SouthEast:
                mapX--;
                mapY++;
                transform.rotation = Quaternion.Euler(0, 0, 225);
                transform.position += new Vector3(pixelHeight, -pixelHeight, 0f);//down right
                break;
            default:
                //Debug.Log("WTF invalid move!");
                break;
        }

    }

    protected virtual void Kill()
    {
        //Talk to spawn
        Debug.Log(type);
        spawnControl.Spawn(-type);
        Destroy(this.gameObject);
    }

    protected void LoadPos (){
        //Store the initial posistion
        posX = transform.position.x;
        posY = transform.position.y;

        //Convert on cavas position into map position
        mapX = (int)(212 - (posX / pixelHeight));
        mapY = (int)(212 - (posY / pixelHeight));
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
