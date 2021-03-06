﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class SandpitDepthView : MonoBehaviour {

    private int width;
    private int height;

    private byte[] startMap;    //Obsticle layout 
    private byte[] finalMap;    //Obsticle layout that is complete and outputted
    private byte[] colourDepth; //Coloured terrain textures
    public ushort[] depth;      ///////DEPTH DATA FROM KINETIC//////////////////////
    public ushort[] defaultMap; ///////DEPTH DATA FROM LOCAL SAVE(one frame)////////
    private ushort[] standardDepth; //a set of standard depth used for calibrations
    private bool[] maskingLayer;    //a circular masking layout that chooses what to render
    private bool[] lavaMap; //a masking layer for the lava

	private float delay = 0.4f; //Every two seconds
	private float timer = 0f;

    //Lava data
    private int lavaX = 212;
    private int lavaY = 212;
    private bool lavaStart = false;
    private int count = 1;
	private int[] lavaHeatMap;


	public PathFinder pf;

    //The upper and lower limit in mm
    public const ushort min = 1170;
    public const ushort max = 1320;

    private Texture2D texture;
    private Mesh mesh;

    public GameObject paintMe;

    public DepthSourceManager dsm;

    public ArrayToFile atf;

    private bool once = true;

    //true if from kinect, false if read from files
    private bool fromText = false; 

    private Renderer renderer;

    //View port variable
    public int viewScale = 212;
    public int viewX = 212;
    public int viewY = 212;
	public int maxHeight = 0;
	public ushort highestPoint = 0;
	public ushort lowestPoint = 0;


	// Use this for initialization
	void Start () {
        this.renderer = GetComponent<Renderer>();
        defaultMap = atf.GetData();
	}
	
	// Update is called once per frame
	void Update () {
        if (once)
        {
            //Debug.Log(dsm.Width);
            //Debug.Log(dsm.Height);
            once = false;

            //initializing all the lists
			startMap = new byte[424 * 424]; //the map that constantly gets changed on frame update
			finalMap = new byte[424 * 424]; //the obsticle map passed into the pathfinder
			colourDepth = new byte[424 * 424 * 4];
            texture = new Texture2D(424, 424, TextureFormat.RGBA32, false);
            standardDepth = new ushort[424 * 424];
            maskingLayer = new bool[424 * 424];
            lavaMap = new bool[424 * 424];
            DrawMaskingCircle();
            return;
        }

        
        if (fromText == true)
        {
            depth = dsm.GetData();
        }
        else
        {
            depth = defaultMap;
        }
        
        /////Updating map with Kinect/////
        //Putting kinect data into an array
        int m = 0;
        //The loop which converts depth into terran colour
        for (int i = 0; i < 424; i++)
        {
            for (int j = 0; j < 512;j++)
            {
                int k = j + 512 * i;
                if (k > 43+(512*i) && k < 468+(512*i))
                {
                    //Debug.Log("test");
                    if (depth[k] == null)
                    {
                        Debug.Log("FUCK!");
                    }
                    else
                    {
                        Mapcolour(depth[k], m);
                        m++;
                    }
                }           
            }
               
        }

        //Debug.Log(depth.Length + " " + a + " " + b);

        texture.LoadRawTextureData(colourDepth);
        texture.Apply();
        renderer.material.mainTexture = texture;

        finalMap = startMap;

        timer += Time.deltaTime;
        if (timer > delay && lavaStart == true)
        {
            count+=2;
			Debug.Log(count);
			getLavaMap();
            timer = 0;
        }
	

		//getting value of highest point of sand
	
		//Debug.Log (highestPoint);
		//Debug.Log (lowestPoint);

	}

    private void Mapcolour(ushort depth, int i)
    {
        //Distance between min and depth
        ushort thisDepth = depth;
        float layerDepth = (max - min) / 5; 
        thisDepth -= min;
		int seaMonsterDepth = 40;
        float height = (float)max - (float)depth;
        if (maskingLayer[i] == true) {

			if (lavaMap[i] == true && depth >= min && depth < (min +(count*0.8)) && depth < max){

				colourDepth[i * 4 + 0] = (byte)(250 - count);//(byte)(255 - (50 * thisDepth / (layerDepth)));
				colourDepth[i * 4 + 1] = 0;//(byte)(255 - (50 * thisDepth / (layerDepth)));
				colourDepth[i * 4 + 2] = 0;//(byte)(255 - (50 * thisDepth / (layerDepth)));					
				colourDepth[i * 4 + 3] = 250;						
				startMap[i] = 6;
			} 

            else if (depth > min && depth <= (min + layerDepth))
            {
                //Volcano
                colourDepth[i * 4 + 0] = 250;//(byte)(255 - (50 * thisDepth / (layerDepth)));
                colourDepth[i * 4 + 1] = 0;//(byte)(255 - (50 * thisDepth / (layerDepth)));
                colourDepth[i * 4 + 2] = 0;//(byte)(255 - (50 * thisDepth / (layerDepth)));
                colourDepth[i * 4 + 3] = 250;
                startMap[i] = 6;
            }
			else if (depth > (min + layerDepth) && depth < (min + layerDepth*2))
			{
				//If the depth is above the boundary, snowy peaks
				colourDepth[i * 4 + 0] = 255;//(byte)(255 - (50 * thisDepth / (layerDepth)));
				colourDepth[i * 4 + 1] = 255;//(byte)(255 - (50 * thisDepth / (layerDepth)));
				colourDepth[i * 4 + 2] = 255;//(byte)(255 - (50 * thisDepth / (layerDepth)));
				colourDepth[i * 4 + 3] = 255;
				startMap[i] = 5;
			}
            else if (depth < (max - layerDepth * 2) && depth > (max - layerDepth * 3))
            {
                //Barren mountain ranges
                colourDepth[i * 4 + 0] = 150;
                colourDepth[i * 4 + 1] = 72;
                colourDepth[i * 4 + 2] = 0;
                colourDepth[i * 4 + 3] = 255;
                startMap[i] = 4;
            }
            else if (depth < (max - layerDepth) && depth > (max - layerDepth * 2))
            {
                //Forest and woodland
                colourDepth[i * 4 + 0] = 25;
                colourDepth[i * 4 + 1] = (byte)(255f - (105 * (float)((height - layerDepth) / layerDepth)));//150;
                colourDepth[i * 4 + 2] = 15;
                colourDepth[i * 4 + 3] = 150;
                startMap[i] = 3;
            }
            else if (depth < max && depth > (max - layerDepth))
            {
                //Shallow bank and grassland
                colourDepth[i * 4 + 0] = (byte)(255f - (255f * (float)((height) / layerDepth)));
                colourDepth[i * 4 + 1] = (byte)225;
                colourDepth[i * 4 + 2] = 5;
                colourDepth[i * 4 + 3] = 255;
                startMap[i] = 2;
            }
			else if (depth >= max && depth < max+seaMonsterDepth)
            {
                //Water
                colourDepth[i * 4 + 0] = 0;
                colourDepth[i * 4 + 1] = (byte)(50f + (25f * (float)((height) / layerDepth)));
                colourDepth[i * 4 + 2] = (byte)(255f + (155 * (float)((height) / layerDepth)));
                colourDepth[i * 4 + 3] = 255; //(byte)(100 *(float)((height) / layerDepth));
                startMap[i] = 1;
            }
			else if (depth >= max+seaMonsterDepth){
				//deep water
				colourDepth[i * 4 + 0] = 0;
				colourDepth[i * 4 + 1] = (byte)(50f + (25f * (float)((height) / layerDepth)));
				colourDepth[i * 4 + 2] = (byte)(255f + (155 * (float)((height) / layerDepth)));
				colourDepth[i * 4 + 3] = 255; //(byte)(100 *(float)((height) / layerDepth));
				startMap[i] = 7;
			}
            else
            {
                colourDepth[i * 4 + 0] = 0;
                colourDepth[i * 4 + 1] = 0;
                colourDepth[i * 4 + 2] = 0;
                colourDepth[i * 4 + 3] = 0;
                startMap[i] = 0;
            }
        }
        else
        {
            colourDepth[i * 4 + 0] = 0;//(byte)(255 - (50 * thisDepth / (layerDepth)));
            colourDepth[i * 4 + 1] = 0;//(byte)(255 - (50 * thisDepth / (layerDepth)));
            colourDepth[i * 4 + 2] = 0;//(byte)(255 - (50 * thisDepth / (layerDepth)));
            colourDepth[i * 4 + 3] = 0;
            startMap[i] = 0;
        }

    }


    public byte[] getMap(){
        return finalMap;
    }

    //Control button functions
    public void toggleKineticOn()
    {
        if (fromText == true)
        {
            fromText = false;
        }
        else
        {
            fromText = true;
        }
    }

    //Takes a terrain type and returns the array position to spawn to
    public int getRandomSpawnLocation(byte terrainType)
    {
        int terrainQuantity = 0;
        int[] terrainPos = new int[finalMap.Length];
        int j = 0;

        for (int i = 0; i < finalMap.Length; i++)
        {
            if (finalMap[i] == terrainType)
            {
                terrainQuantity += 1;
                terrainPos[j] = i;
                j++;
            }
        }

        if (terrainQuantity > 0)
        {
            //If there is at least one valid location
            int thing = UnityEngine.Random.Range(0, terrainQuantity);

            return terrainPos[thing];
        }
        return -1;
    }

    public void DrawMaskingCircle()
    {
        //The loop which converts depth into terran colour
        for (int i = 0; i < 423; i++)
        {
            for (int j = 0; j < 423; j++)
            {
                //Debug.Log("test");
                if (isInCircle(j, i, viewX, viewY, viewScale) == true)
                {
                    maskingLayer[i * 424 + (j + 1)] = true;
                }
                else
                {
                    maskingLayer[i * 424 + (j + 1)] = false;
                }
            }

        }
    }

    //Takes x and y values and checks if position is inside circle of certain scale
    public bool isInCircle(int x, int y, int viewX, int viewY, int viewScale)
    {
        if(Math.Pow(x - viewX, 2) + Math.Pow(y - viewY, 2) <= Math.Pow(viewScale, 2)){
            //print("inCircle");
            return true;
        }
        else{
            return false;
        }
    }


    private void getLavaMap()
    {
        if (lavaStart == false)
        {
            return;
        }
        //Debug.Log("test");
        //The loop which converts depth into terran colour
        for (int i = 0; i < 423; i++)
        {
            for (int j = 0; j < 423; j++)
            {
                //Debug.Log("test");
                if (isInCircle(j, i, lavaX, lavaY, (2*count)/3) == true && count < 250)
                {
                    lavaMap[i * 424 + (j + 1)] = true;
                }
                else
                {
                    lavaMap[i * 424 + (j + 1)] = false;
                }
            }

        }
    }

    public void volcanoStart(int x, int y)
    {
        Debug.Log("starting");
        lavaX = x;
        lavaY = y;
        lavaStart = true;
    }

}
