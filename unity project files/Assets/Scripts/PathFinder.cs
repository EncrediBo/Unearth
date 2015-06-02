using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class PathFinder : MonoBehaviour {
    public SandpitDepthView sdv;

    //Time interval for recalculation
    public float delay = 2f; //Every two seconds
    public float timer = 0f;

    //Heatmap used for AI pathfinding
    private static byte[] finalMap;
    private int[] heatMap1;
    private int[] heatMap2;
	private int[] heatMap3;
    private int[] heatMapLandPlant;

    //Destination coordinates
    private int finalX = 180;
    private int finalY = 180;

    //Starting coordinates for heatmaps
    private int startX = 270;
    private int startY = 270;

	public SpawnControl sc;

	private int seaMonsterX = 0;
	private int seaMonsterY = 0;

    private static PathFinder _instance;

    public static PathFinder Instance()
    {
        if (_instance == null)
        {
            Debug.Log("PathFinder: Instance is NULL. Run around in circles and panic.");
            _instance = new PathFinder();
        }
        return _instance;

    }

	// Use this for initialization
	void Start () {
        heatMap1 = new int[424*424];
        heatMap2 = new int[424*424];
		heatMap3 = new int[424*424];
		for (int i = 0; i < heatMap1.Length; i++)
        {
            heatMap1[i] = -1;
            heatMap2[i] = -1;
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
            //Debug.Log("tick");
            timer = 0f;
        }
        else
        {
            return;
        }

        //Draw a new heat map according to obsticle map and destination
        finalMap = sdv.getMap();


        if (doingStuff == false)
        {
            DrawHeatMap();
        }
	}

    public int[] getHeatMap(int i)
    {
        switch (i)
        {
            case 1:
                //Land plant
                return heatMap2;
            case 2:
                //Sea plant
                return heatMap1;
            case 3:
                //Land animal
                return heatMap2;
            case 4:
                //Sea animal
                return heatMap1;
            default:
                return heatMap1;
        }
    }




    private void DrawHeatMap()
    {
        doingStuff = true;
		seaMonsterX = sc.getSeaMonsterX();
		seaMonsterY = sc.getSeaMonsterY();
        flood(finalX, finalY, 10000, 2); //Movement for land
		if (seaMonsterX != 0 && seaMonsterY != 0) {
			floodAgain (seaMonsterX, seaMonsterY, 10000, 1); //Movement for sea
		}
    }

    private void flood(int xStart, int yStart, int heatStart, byte level)
    {
        for (int i = 0; i < heatMap2.Length; i++)
        {
            heatMap2[i] = 0;
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

            if (heatMap2[y * 424 + x] == 0)
            {
                heatMap2[y * 424 + x] = heat;

                if ((x - 1) > 0 && finalMap[y * 424 + (x - 1)] >= level && heatMap2[y * 424 + (x - 1)] == 0)
                {
                    xQueue.Enqueue(x - 1);
                    yQueue.Enqueue(y);
                    heatQueue.Enqueue(heat - 1);
                }
                if ((x + 1) < 423 && finalMap[y * 424 + (x + 1)] >= level && heatMap2[y * 424 + (x + 1)] == 0)
                {
                    xQueue.Enqueue(x + 1);
                    yQueue.Enqueue(y);
                    heatQueue.Enqueue(heat - 1);
                }

                if ((y - 1) > 0 && finalMap[(y - 1) * 424 + x] >= level && heatMap2[(y - 1) * 424 + x] == 0)
                {
                    xQueue.Enqueue(x);
                    yQueue.Enqueue(y - 1);
                    heatQueue.Enqueue(heat - 1);
                }
                if ((y + 1) < 423 && finalMap[(y + 1) * 424 + x] >= level && heatMap2[(y + 1) * 424 + x] == 0)
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

    private void floodAgain(int xStart, int yStart, int heatStart, byte level)
    {
        for (int i = 0; i < heatMap1.Length; i++)
        {
            heatMap1[i] = 0;
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

            if (heatMap1[y * 424 + x] == 0)
            {
                heatMap1[y * 424 + x] = heat;

                if ((x - 1) > 0 && finalMap[y * 424 + (x - 1)] == level && heatMap1[y * 424 + (x - 1)] == 0 || finalMap[y * 424 + (x - 1)] == 7)
                {
                    xQueue.Enqueue(x - 1);
                    yQueue.Enqueue(y);
                    heatQueue.Enqueue(heat - 1);
                }
                if ((x + 1) < 423 && finalMap[y * 424 + (x + 1)] == level && heatMap1[y * 424 + (x + 1)] == 0 || finalMap[y * 424 + (x - 1)] == 7)
                {
                    xQueue.Enqueue(x + 1);
                    yQueue.Enqueue(y);
                    heatQueue.Enqueue(heat - 1);
                }

                if ((y - 1) > 0 && finalMap[(y - 1) * 424 + x] == level && heatMap1[(y - 1) * 424 + x] == 0 || finalMap[y * 424 + (x - 1)] == 7)
                {
                    xQueue.Enqueue(x);
                    yQueue.Enqueue(y - 1);
                    heatQueue.Enqueue(heat - 1);
                }
                if ((y + 1) < 423 && finalMap[(y + 1) * 424 + x] == level && heatMap1[(y + 1) * 424 + x] == 0 || finalMap[y * 424 + (x - 1)] == 7)
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

    public byte getTerrain(int x, int y){
        return finalMap[y * 424 + (x + 1)];
    }

}
