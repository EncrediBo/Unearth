using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandManControl : Animal
{

    // Use this for initialization
    protected override void Start()
    {
        type = 1;
        terrain = 3;
        base.Start();
    }
    protected override void Update()
    {
        if (myStateDuration >= 90)
        {
            ChangeState(State.Seeking);
        }

        //Get terrain map
        terrainMap = pathFinder.sdv.getMap();

        base.Update();
    }

    protected override void CheckFront()
    {
        int x = mapX;
        int y = mapY;
        switch (facing)
        {
            case Direction.NorthWest:
                x = mapX + 2;
                y = mapY - 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain -1))
                {
                    turning = true;
                    facing = Direction.SouthEast;
                }
                break;
            case Direction.North:
                y = mapY - 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain - 1))
                {
                    turning = true;
                    facing = Direction.South;
                }
                break;
            case Direction.NorthEast:
                x = mapX - 2;
                y = mapY - 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain - 1))
                {
                    turning = true;
                    facing = Direction.SouthWest;
                }
                break;
            case Direction.West:
                x = mapX + 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain - 1))
                {
                    turning = true;
                    facing = Direction.East;
                }
                break;
            case Direction.East:
                x = mapX - 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain - 1))
                {
                    turning = true;
                    facing = Direction.West;
                }
                break;
            case Direction.SouthWest:
                x = mapX + 2;
                y = mapY + 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain - 1))
                {
                    turning = true;
                    facing = Direction.NorthEast;
                }
                break;
            case Direction.South:
                y = mapY + 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain - 1))
                {
                    turning = true;
                    facing = Direction.North;
                }
                break;
            case Direction.SouthEast:
                x = mapX - 2;
                y = mapY + 2;
                if (terrainMap[y * 424 + x] != terrain && terrainMap[y * 424 + x] != (terrain - 1))
                {
                    turning = true;
                    facing = Direction.NorthWest;
                }
                break;
            case Direction.DONTMOVE:
                facing = Direction.DONTMOVE;
                break;
            default:
                Debug.Log("WTF invalid move!");
                break;
        }
    }

}
