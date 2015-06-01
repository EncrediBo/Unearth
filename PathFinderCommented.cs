using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class PathFinder : MonoBehaviour {
    public SandpitDepthView sdv;

    //Time interval for recalculation
    public float delay = 2f; //Every half a second
    public float timer = 0f;

    //Heatmap used for AI pathfinding
    private byte[] finalMap;
    private int[] heatMap;
    private int[] landHeatMap;
    private bool[] waterMap;
    private bool[] landMap;

    public int waterMapCount = 0;
    public int waterMapCountTrue = 0;

    //Destination coordinates
    private int finalX = 180;
    private int finalY = 180;

    //Starting coordinates
    private int startX = 270;
    private int startY = 270;

    private static PathFinder _instance;

    public static PathFinder Instance()
    {
        if (_instance == null)
        {
            _instance = new PathFinder();
        }
        return _instance;

    }

	// Use this for initialization
	void Start () {
        heatMap = new int[424*424];
        for (int i = 0; i < heatMap.Length; i++)
        {
            heatMap[i] = -1;
        }
        _instance = this;
	}

    private bool doingStuff = false;
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(heatMap[startY * 424 + startX]);

        //Timer delay
        timer += Time.deltaTime;
        if (timer > delay)
        {
            timer = 0f;
        }
        else
        {
            return;
        }

        //Draw a new heat map according to obsticle map and destination
        finalMap = sdv.getMap();
        //waterMap = sdv.getMap();
        landMap = sdv.getLandMap();

        //waterMapCount = waterMap.Length;
        //waterMapCountTrue = 0;
        /*
        foreach (bool b in waterMap)
        {
            if (b == true)
            {
                //waterMapCountTrue++;
            }
        }*/

        if (doingStuff == false)
        {
            DrawHeatMap();
        }
	}

    public int[] getHeatMap()
    {
        
        return heatMap;
    }

    public int[] getLandHeatMap()
    {
        return landHeatMap;
    }

    private void DrawHeatMap()
    {
        doingStuff = true;
        
        //Putting in the initial heat
        /*for(int i = 0; i < waterMap.Length; i++){
            if(i == (startX * startY)){
                heatMap[i] = 1;
            }
            else
            {
                heatMap[i] = 0;
            }
        }*/

        /*Method 1
        int cycles = 0;
        while (heatMap[startX*startY] == 0)
        {
            //Looping over the entire heatMap except for the tiles at the boundary
            for (int y = 1; y < 423; y++)
            {
                for (int x = 1; x < 423; x++)
                {
                    if (CheckHeat(y * 424 + x) == true && waterMap[y * 424 + x] == true)
                    {
                        heatMap[y * 424 + x] += 1;
                    }
                }
            }

            //Max cycle reached, inifinate loop prevented
            if (cycles >= 100)
            {
                Debug.Log("90000 cycles and still nothing");
                return;
            }
            cycles++;
        }*/


        //floodHeatMap(finalX, finalY, Direction.Start, 1000);

        //StartCoroutine(floodHeatMapQueueBased(finalX, finalY, 1000));

        //floodHeatMapQueueBased(finalX, finalY, 1000);

        //floodHeatMapAgain(finalX, finalY, 1000);

        floodAgain(finalX, finalY, 10000, 2);
    }


    private void floodAgain(int xStart, int yStart, int heatStart, byte level)
    {
        for (int i = 0; i < heatMap.Length; i++)
        {
            heatMap[i] = 0;
        }

        Queue<int> xQueue = new Queue<int>();
        Queue<int> yQueue = new Queue<int>();
        Queue<int> heatQueue = new Queue<int>();

        xQueue.Enqueue(xStart);
        yQueue.Enqueue(yStart);
        heatQueue.Enqueue(heatStart);

        int x;
        int y;
        int heat;

        int breakCount = 0;
        //Debug.Log("Starting");
        while (xQueue.Count > 0)
        {
            //Debug.Log(xQueue.Count);

            breakCount++;

            if (breakCount > 1000)
            {
                //Debug.Log("Breaking");
                //break;
            }

            x = xQueue.Dequeue();
            y = yQueue.Dequeue();
            heat = heatQueue.Dequeue();

            if (heatMap[y * 424 + x] == 0)
            {
                heatMap[y * 424 + x] = heat;

                if ((x - 1) > 0 && finalMap[y * 424 + (x - 1)] == level && heatMap[y * 424 + (x - 1)] == 0)
                {
                    xQueue.Enqueue(x - 1);
                    yQueue.Enqueue(y);
                    heatQueue.Enqueue(heat - 1);
                }
                if ((x + 1) < 423 && finalMap[y * 424 + (x + 1)] == level && heatMap[y * 424 + (x + 1)] == 0)
                {
                    xQueue.Enqueue(x + 1);
                    yQueue.Enqueue(y);
                    heatQueue.Enqueue(heat - 1);
                }

                if ((y - 1) > 0 && finalMap[(y - 1) * 424 + x] == level && heatMap[(y - 1) * 424 + x] == 0)
                {
                    xQueue.Enqueue(x);
                    yQueue.Enqueue(y - 1);
                    heatQueue.Enqueue(heat - 1);
                }
                if ((y + 1) < 423 && finalMap[(y + 1) * 424 + x] == level && heatMap[(y + 1) * 424 + x] == 0)
                {
                    xQueue.Enqueue(x);
                    yQueue.Enqueue(y + 1);
                    heatQueue.Enqueue(heat - 1);
                }
            }
        }
        //Debug.Log("Finishing");
        doingStuff = false;
    }


    public enum Direction {
        North, 
        East,
        South,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest,
        Start
    }
    /*
    private int times;
    private void floodHeatMapAgain(int xStart, int yStart, int heatStart, byte level)
    {
        for (int i = 0; i < heatMap.Length; i++)
        {
            heatMap[i] = 0;
        }
        times = 0;
        flood(xStart, yStart, heatStart, level);
        Debug.Log(times);
    }

    private void flood(int x, int y, int heat, byte level)
    {
        times++;
        if (x > 0 && x < 423 && y > 0 && y < 423)
        {
            if (finalMap[y * 424 + x] == level && heatMap[y * 424 + x] == 0 && heat > 0)
            {
                heatMap[y * 424 + x] = heat;

                if (x - 1 > 0 && y - 1 > 0 && heatMap[(y - 1) * 424 + (x - 1)] == 0)
                {
                    flood(x - 1, y - 1, heat - 1, level);
                }
                if (x - 1 > 0 && y + 1 < 423 && heatMap[(y + 1) * 424 + (x - 1)] == 0)
                {
                    flood(x - 1, y + 1, heat - 1, level);
                }
                if (x + 1 < 423 && y - 1 > 0 && heatMap[(y - 1) * 424 + (x + 1)] == 0)
                {
                    flood(x + 1, y - 1, heat - 1, level);
                }
                if (x + 1 < 423 && y + 1 < 423 && heatMap[(y + 1) * 424 + (x + 1)] == 0)
                {
                    flood(x + 1, y + 1, heat - 1, level);
                }

                if (x - 1 > 0 && heatMap[y * 424 + (x - 1)] == 0)
                {
                    flood(x - 1, y, heat - 1, level);
                }
                if (x + 1 < 423 && heatMap[y * 424 + (x + 1)] == 0)
                {
                    flood(x + 1, y, heat - 1, level);
                }
                if (y - 1 > 0 && heatMap[(y - 1) * 424 + x] == 0)
                {
                    flood(x, y - 1, heat - 1, level);
                }
                if (y + 1 < 423 && heatMap[(y + 1) * 424 + x] == 0)
                {
                    flood(x, y + 1, heat - 1, level);
                }
            }
        }
        else
        {
            //Debug.Log("outside");
        }
    }

    //public List<Vector2> queue;
    //public List<Vector2> searched;
    //public int queueCount = 0;
    
        private IEnumerator floodHeatMapQueueBased(int xStart, int yStart, int heatStart)
        {
            CoroutineHelper.Start();
            queueCount = 0;
            Debug.Log("start");
            for (int i = 0; i < heatMap.Length; i++)
            {
                heatMap[i] = 0;
            }

            List<Vector2>  queue = new List<Vector2>();
            Queue<int> heatQueue = new Queue<int>();

            queue.Add(new Vector2(xStart, yStart));
            heatQueue.Enqueue(heatStart);


            while (queue.Count > 0)
            {
                queueCount = queue.Count;
                if (CoroutineHelper.Exceeded(20))
                {
                    yield return null;
                    CoroutineHelper.Start();
                }
                Vector2 current = queue[0];
                queue.RemoveAt(0);
                int heat = heatQueue.Dequeue();

                //searched.Add(current);
            

                int x = Mathf.FloorToInt(current.x);
                int y = Mathf.FloorToInt(current.y);

                if (heatMap[y * 424 + x] != 0 || waterMap[y * 424 + x] == false)
                {
                    continue;
                }

                //Debug.Log("current: "+current);

                heatMap[y * 424 + x] = heat;

                Vector2 newSpot;

                if ((x - 1) > 0)
                {
                    newSpot = new Vector2(x - 1, y);
                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[y * 424 + (x - 1)] == 0 && waterMap[y * 424 + (x - 1)] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }

                if ((x + 1) < 423)
                {
                    newSpot = new Vector2(x + 1, y);

                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[y * 424 + (x + 1)] == 0 && waterMap[y * 424 + (x + 1)] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }

                if ((y - 1) > 0)
                {
                    newSpot = new Vector2(x, y - 1);

                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[(y - 1) * 424 + x] == 0 && waterMap[(y - 1) * 424 + x] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }

                if ((y + 1) < 423)
                {
                    newSpot = new Vector2(x, y + 1);

                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[(y + 1) * 424 + x] == 0 && waterMap[(y + 1) * 424 + x] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }

                if ((x - 1) > 0 && (y - 1) > 0)
                {
                    newSpot = new Vector2(x - 1, y - 1);

                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[(y - 1) * 424 + (x - 1)] == 0 && waterMap[(y - 1) * 424 + (x - 1)] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }

                if ((x - 1) > 0 && (y + 1) < 423)
                {
                    newSpot = new Vector2(x - 1, y + 1);

                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[(y + 1) * 424 + (x - 1)] == 0 && waterMap[(y + 1) * 424 + (x - 1)] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }

                if ((x + 1) < 423 && (y - 1) > 0)
                {
                    newSpot = new Vector2(x + 1, y - 1);

                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[(y - 1) * 424 + (x + 1)] == 0 && waterMap[(y - 1) * 424 + (x + 1)] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }

                if ((x + 1) < 423 && (y + 1) < 423)
                {
                    newSpot = new Vector2(x + 1, y + 1);

                    //Debug.Log("newspot: " + newSpot);
                    if (heatMap[(y + 1) * 424 + (x + 1)] == 0 && waterMap[(y + 1) * 424 + (x + 1)] == true)
                    {
                        queue.Add(newSpot);
                        heatQueue.Enqueue(heat - 1);
                    }
                }
            }

            Debug.Log("done");
            Debug.Log(heatMap[startY * 424 + startX]);
            doingStuff = false;
        }

        private void floodHeatMap(int x, int y, Direction dir, int heat)
        {
            //If the function lands on an boundary or obsticle tile
            if (x < 0) return;
            if (x >= 424) return;
            if (y < 0) return;
            if (y >= 424) return;
            if (waterMap[y * 424 + x] == false)
            {
                Debug.Log(false);
                heatMap[y * 424 + x] = 0;
                return;
            }

            //If the function lands on the exit tile
            if (x == startX && y == startY)
            {
                Debug.Log("Found it!");
            }

            heatMap[y * 424 + x] = heat;

            switch (dir)
            {
                case Direction.Start:
                    floodHeatMap(x - 1, y - 1, Direction.NorthWest, heat - 1);
                    floodHeatMap(x + 1, y - 1, Direction.NorthEast, heat - 1);
                    floodHeatMap(x - 1, y + 1, Direction.SouthWest, heat - 1);
                    floodHeatMap(x + 1, y + 1, Direction.SouthEast, heat - 1);

                    floodHeatMap(x, y - 1, Direction.North, heat - 1);
                    floodHeatMap(x, y + 1, Direction.South, heat - 1);
                    floodHeatMap(x - 1, y, Direction.West, heat - 1);
                    floodHeatMap(x + 1, y, Direction.East, heat - 1);
                    break;
                case Direction.North:
                    floodHeatMap(x, y - 1, Direction.North, heat - 1);
                    break;
                case Direction.NorthEast:
                    floodHeatMap(x + 1, y - 1, Direction.NorthEast, heat - 1);
                    floodHeatMap(x, y - 1, Direction.North, heat - 1);
                    floodHeatMap(x + 1, y, Direction.East, heat - 1);
                    break;
                case Direction.NorthWest:
                    floodHeatMap(x - 1, y - 1, Direction.NorthWest, heat - 1);
                    floodHeatMap(x, y - 1, Direction.North, heat - 1);
                    floodHeatMap(x - 1, y, Direction.West, heat - 1);
                    break;
                case Direction.West:
                    floodHeatMap(x - 1, y, Direction.West, heat - 1);
                    break;
                case Direction.East:
                    floodHeatMap(x + 1, y, Direction.East, heat - 1);
                    break;
                case Direction.SouthEast:
                    floodHeatMap(x + 1, y + 1, Direction.SouthEast, heat - 1);
                    floodHeatMap(x, y + 1, Direction.South, heat - 1);
                    floodHeatMap(x + 1, y, Direction.East, heat - 1);
                    break;
                case Direction.South:
                    floodHeatMap(x, y + 1, Direction.South, heat - 1);
                    break;
                case Direction.SouthWest:
                    floodHeatMap(x - 1, y + 1, Direction.SouthWest, heat - 1);
                    floodHeatMap(x, y + 1, Direction.South, heat - 1);
                    floodHeatMap(x - 1, y, Direction.West, heat - 1);
                    break;
            }
        }

        private bool CheckHeat(int i)
        {
            //Total heat of surrounding pixels
            int totalHeat = heatMap[i-425] + heatMap[i-424] + heatMap[i-423] + heatMap[i-1] + heatMap[i] 
                + heatMap[i+1] + heatMap[i+423] + heatMap[i+424] + heatMap[i+425];

            //Only one heat from any of the eight direction is needed to warm up this tile
            if (totalHeat > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/











}
