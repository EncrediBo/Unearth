using UnityEngine;
using System.Collections;

public class SandpitDepthView : MonoBehaviour {

    private int width;
    private int height;

    private byte[] startMap;
    private byte[] finalMap;
    private byte[] colourDepth;
    public ushort[] depth;
    public ushort[] defaultMap;

    //The upper and lower limit in mm
    public const ushort min = 1200;
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

	}

    private void Mapcolour(ushort depth, int i)
    {
        //Distance between min and depth
        ushort thisDepth = depth;
        float layerDepth = (max - min) / 4; 
        thisDepth -= min;
        float height = (float)max - (float)depth;

        if (depth > min && depth <= (min + layerDepth))
        {
            //If the depth is above the boundary, snowy peaks
            colourDepth[i * 4 + 0] = 255;//(byte)(255 - (50 * thisDepth / (layerDepth)));
            colourDepth[i * 4 + 1] = 255;//(byte)(255 - (50 * thisDepth / (layerDepth)));
            colourDepth[i * 4 + 2] = 255;//(byte)(255 - (50 * thisDepth / (layerDepth)));
            colourDepth[i * 4 + 3] = 255;
            startMap[i] = 5;
        }
        else if (depth < (max - layerDepth*2) && depth > (max - layerDepth*3))
        {
            //Barren mountain ranges
            colourDepth[i * 4 + 0] = (byte)150;
            colourDepth[i * 4 + 1] = (byte)150;
            colourDepth[i * 4 + 2] = 150;
            colourDepth[i * 4 + 3] = 255;
            startMap[i] = 4;
        }
        else if (depth < (max - layerDepth) && depth > (max - layerDepth*2))
        {
            //Forest and woodland
            colourDepth[i * 4 + 0] = 25;
            colourDepth[i * 4 + 1] = (byte)(255f - (105 * (float)((height- layerDepth) / layerDepth)));//150;
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
        else if (depth >= max)
        {
            //Water
            colourDepth[i * 4 + 0] = 0;
            colourDepth[i * 4 + 1] = (byte)(50f +(25f * (float)((height) / layerDepth)));
            colourDepth[i * 4 + 2] = (byte)(255f +(155 * (float)((height) / layerDepth)));
            colourDepth[i * 4 + 3] = 255; //(byte)(100 *(float)((height) / layerDepth));
            startMap[i] = 1;
        }
        else
        {
            colourDepth[i * 4 + 0] = 0;
            colourDepth[i * 4 + 1] = 0;
            colourDepth[i * 4 + 2] = 0;
            colourDepth[i * 4 + 3] = 255;
            startMap[i] = 0;
        }
    }

    private void fillColour(int i, ushort depth)
    {

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

        int thing = Random.Range(0, terrainQuantity);

        return terrainPos[thing];
    }
}
