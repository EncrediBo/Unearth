using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Animal : MonoBehaviour
{

    public PathFinder pathFinder;
    public SpawnControl spawnControl;
	protected Animator animator;

    //public static int count = 1;

    //Creature type
    /*
     * 1 = landMan
     * 2 = seaMan
     * 
     */
    protected int type = 1; //type of creature
    protected byte terrain = 1; //terrain preference

    //location in game space in unity units
    protected float posX;
    protected float posY;

    //location in the map
    protected int mapX;
    protected int mapY;

    //Direction of this sprite is facing
    protected Direction facing;
    protected bool turning = false;

    protected State myState = State.Idle;
    protected int myStateDuration = 0;

    //Movement speed delay
    //Time interval for recalculation
    private float delay = 0.02f; //Every two seconds
    private float timer = 0f;

    protected int[] heatMap;
    protected byte[] terrainMap;

    protected const float pixelHeight = 0.023585f;
 
    // Use this for initialization
    protected virtual void Start()
    {
        //Load the position and map position
        LoadPos();

		//Set the animator
		animator = GetComponent<Animator> ();

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
        myStateDuration++;

        //Get terrain map
        terrainMap = pathFinder.sdv.getMap();

        //Choose which heat map as path finding guide
        heatMap = pathFinder.getHeatMap(type);

        //Applying position, just in case it is bumped
        LoadPos();

        CheckTerrain();

        if (heatMap[mapY * 424 + mapX] == 10000)
        {
            Kill();
            //ChangeState(State.Eating);
        }

        //Animal behaves according to state
        if (myState == State.Hunting)
        {
            //Debug.Log("Hunting");
            Move(checkHeat(mapX, mapY, heatMap));
        }
        else if(myState == State.Seeking)
        {
            //Debug.Log("seeking");
            CheckFront();
            Move(RandomMove());
        }
        else
        {
            return;
        }
    }

    protected virtual void CheckTerrain()
    {
    }

    protected virtual bool CheckFront()
    {
        int x = mapX;
        int y = mapY;
        switch (facing)
        {
            case Direction.NorthWest:
                x = mapX+2;
                y = mapY-2;
                if(terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.SouthEast;
                    return false;
                }
                break;
            case Direction.North:
                y = mapY-2;
                if (terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.South;
                    return false;
                }
                break;
            case Direction.NorthEast:
                x = mapX - 2;
                y = mapY - 2;
                if (terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.SouthWest;
                    return false;
                }
                break;
            case Direction.West:
                x = mapX+2;
                if (terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.East;
                    return false;
                }
                break;
            case Direction.East:
                x= mapX-2;
                if (terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.West;
                    return false;
                }
                break;
            case Direction.SouthWest:
                x = mapX+2;
                y = mapY+2;
                if (terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.NorthEast;
                    return false;
                }
                break;
            case Direction.South:
                y = mapY + 2;
                if (terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.North;
                    return false;
                }
                break;
            case Direction.SouthEast:
                x= mapX-2;
                y= mapY+2;
                if (terrainMap[y * 424 + x] != terrain)
                {
                    turning = true;
                    facing = Direction.NorthWest;
                    return false;
                }
                break;
            case Direction.DONTMOVE:
                facing = Direction.DONTMOVE;
                break;
            default:
                Debug.Log("WTF invalid move!");
                break;
        }
        return true;
    }
    protected void ChangeState(State state)
    {
        //Debug.Log("Changing state");
        //Debug.Log(state);
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
        if(coll.tag.Equals("Food")){
            //ChangeState(State.Eating);
        } else {
           // print("Ouch");
        }
        //print(coll);
        //coll.gameObject.SetActive(false);
    }

    protected Direction checkHeat (int x, int y, int[] heatMap)
    {
        int curr = heatMap[y * 424 + x];
        int newHeight;
        Direction myDirection = Direction.DONTMOVE;

        //Determine which direction this is heading according to heat and current direction
        if ((x - 1) > 0)
        {
            newHeight = heatMap[y * 424 + (x - 1)];

            if (newHeight > curr)
            {
                curr = newHeight;
                myDirection = Direction.East;
            }
            else if (newHeight == curr && facing == Direction.East && curr > 0)
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
            else if (newHeight == curr && facing == Direction.West && curr > 0)
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
            else if (newHeight == curr && facing == Direction.North && curr > 0)
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
            else if (newHeight == curr && facing == Direction.NorthEast && curr > 0)
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
            else if (newHeight == curr && facing == Direction.NorthWest && curr > 0)
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
            else if (newHeight == curr && facing == Direction.South && curr > 0)
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
            else if (newHeight == curr && facing == Direction.SouthWest && curr > 0)
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
            else if (newHeight == curr && facing == Direction.SouthEast && curr > 0)
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

        //Debug.Log(mapX);
        //Debug.Log(mapY);
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

        if (turning == true)
        {
            if (CheckFront() == false)
            {
                return;
            }
        }

        switch (dir)
        {
            case Direction.NorthWest:
                facing = Direction.NorthWest;
                transform.rotation = Quaternion.Euler(0, 0, 45);
                transform.position += new Vector3(-pixelHeight, pixelHeight, 0f);//up left
                break;
            case Direction.North:
                facing = Direction.North;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.position += new Vector3(0f, pixelHeight, 0f);//up
                break;
            case Direction.NorthEast:
                facing = Direction.NorthEast;
                transform.rotation = Quaternion.Euler(0, 0, 315);
                transform.position += new Vector3(pixelHeight, pixelHeight, 0f);//up right
                break;
            case Direction.West:
                facing = Direction.West;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                transform.position += new Vector3(-pixelHeight, 0f, 0f);//left
                break;
            case Direction.East:
                facing = Direction.East;
                transform.rotation = Quaternion.Euler(0, 0, 270);
                transform.position += new Vector3(pixelHeight, 0f, 0f);//right
                break;
            case Direction.SouthWest:
                facing = Direction.SouthWest;
                transform.rotation = Quaternion.Euler(0, 0, 135);
                transform.position += new Vector3(-pixelHeight, -pixelHeight, 0f);//down left
                break;
            case Direction.South:
                facing = Direction.South;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                transform.position += new Vector3(0f, -pixelHeight, 0f);//down
                break;
            case Direction.SouthEast:
                facing = Direction.SouthEast;
                transform.rotation = Quaternion.Euler(0, 0, 225);
                transform.position += new Vector3(pixelHeight, -pixelHeight, 0f);//down right
                break;
            case Direction.DONTMOVE:
                facing = Direction.DONTMOVE;
                break;
            default:
                //Debug.Log("WTF invalid move!");
                break;
        }

    }

    protected Direction RandomMove()
    {
        //If the facing is not set yet, go random direction
        if (facing == Direction.DONTMOVE){
            int randomDir= Random.Range(1, 8);
            //Debug.Log(facing);
            return NumToDir(randomDir);
        }

        //When the change direction was just triggered, don't 
        if (turning == true)
        {
            turning = false;
            return facing;
        }

        int rnd = Random.Range(1, 208);
        if (rnd > 200){
            //Debug.Log("random go");
            //Debug.Log("going wiht me guts");
            turning = true;
            return NumToDir(rnd - 200);
        }
        else
        {
           //Debug.Log(facing);
           // Debug.Log("keep going");
            return facing;
        }
    }

    protected Direction NumToDir(int num){
        switch (num)
        {
            case 1:
                return Direction.North;
            case 2:
                return Direction.NorthEast;
            case 3:
                return Direction.NorthWest;
            case 4:
                return Direction.East;
            case 5:
                return Direction.West;
            case 6:
                return Direction.South;
            case 7:
                return Direction.SouthEast;
            case 8:
                return Direction.SouthWest;
            default:
                Debug.Log("Invalid number to direction input");
                return Direction.DONTMOVE;
        }
    }

    protected virtual void Kill()
    {
        //Talk to spawn
        //Debug.Log(type);
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
        LoadPos();
        return mapX;
    }

    public int getLocationY()
    {
        LoadPos();
        return mapY;
    }
}
