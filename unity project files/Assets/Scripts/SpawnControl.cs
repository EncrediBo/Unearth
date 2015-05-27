﻿using UnityEngine;
using System.Collections;

public class SpawnControl : MonoBehaviour {

    //public SandpitDepthView sdv;
    private byte[] finalMap;

    //Prefabs of all the creatures/plants that can be spawned
    public GameObject seaMan;
    public GameObject landMan;

    public SandpitDepthView sdv;

    //Prefabs for trees
    public GameObject[] trees;									//Array of tree prefabs.

    //All the data keeping track of the stuff that has spawned
    private int seaManCount = 0;
    private int seaManMax = 55;
    private int landManCount = 0;
    private int landManMax = 5;
    private int landPlantCount = 0;
    private int landPlantMax = 55;
    private static bool seaManSpawn = true;
    private static bool landManSpawn = false;
    private static bool landPlantSpawn = true;

    protected const float pixelHeight = 0.023585f;

    private static SpawnControl _instance;

    public static SpawnControl Instance()
    {
        if (_instance == null)
        {
            Debug.Log("SpawnControl: Instance is NULL. Run around in circles and panic.");
            _instance = new SpawnControl();
        }
        return _instance;

    }

	// Use this for initialization
	void Start () {
        //Instantiate(seaMan);
        //Instantiate(landMan);
        _instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        //finalMap = sdv.getMap();
        //Debug.Log("seamancount smaller than seamanmax: "+ (seaManCount < seaManMax));

        if (seaManSpawn == true && seaManCount < seaManMax)
        {
            int spawnLoc = sdv.getRandomSpawnLocation(2); //get spawn location in ushort
            //convert to x and y values
            int x = spawnLoc % 424;
            int y = spawnLoc / 424;
            //convert to canvas values
            float mapX = (212 - x) * pixelHeight;
            float mapY = (212 - y) * pixelHeight;

            Debug.Log(seaManCount);
            Debug.Log(seaManMax);
            Instantiate(seaMan, new Vector3(mapX, mapY, 0f), Quaternion.identity);
            seaManCount++;
            //seaManSpawn = false;
        }

        if (landPlantSpawn == true && landPlantCount < landPlantMax)
        {
            int spawnLoc = sdv.getRandomSpawnLocation(1); //get spawn location in ushort
            //convert to x and y values
            int x = spawnLoc % 424;
            int y = spawnLoc / 424;
            //convert to canvas values
            float mapX = (212 - x) * pixelHeight;
            float mapY = (212 - y) * pixelHeight;

            //Debug.Log(seaManCount);
            //Debug.Log(seaManMax);
            Instantiate(trees[Random.Range(0, trees.Length)], new Vector3(mapX, mapY, 0f), Quaternion.identity);
            landPlantCount++;
            //seaManSpawn = false;
        }
       

        //Instantiate(seaMan);
	}


    void Spawn(int type)
    {
        switch (type)
        {
            case -1:
                landManCount--;
                break;
            case 1:
                if (landManSpawn == true && landManCount < landManMax)//&& landManCount < landManMax)
                {
                    Instantiate(landMan);
                    landManCount++;
                    landManSpawn = false;
                }
                break;
            case -2:
                seaManCount--;
                break;
            case 2:
                if (landManSpawn == true && landManCount < landManMax)//&& landManCount < landManMax)
                {
                    Instantiate(landMan);
                    landManCount++;
                    landManSpawn = false;
                }
                break;
            default:
                break;
        }
    }

    

}
