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
    //protected int type = 1;

    //location in game space in pixels
    protected float posX;
    protected float posY;

    //location in the map
    protected int mapX;
    protected int mapY;

    //Direction of this sprite is facing
    private Direction facing;

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

        //Applying position, just in case it is bumped
        LoadPos();

        //Spawning code
        if (heatMap[mapY * 424 + mapX] == -1)//|| heatMap[mapY * 424 + mapX] == 0)
        {
            return;
        }

        if (heatMap[mapY * 424 + mapX] == 10000)
        {
            //spawnControl.Spawn(type);
            Destroy(this.gameObject);
            //spawnControl.Spawn(-(type));

        }
        else if (heatMap[mapY * 424 + mapX] == 9900)
        {
            //spawnControl.Spawn(type);
        }
        else if (heatMap[mapY * 424 + mapX] == 9850)
        {
            //don't know yet
        }

        //moves character using current position and the heatmap choosen
        Move(checkHeat(mapX, mapY, heatMap));
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
