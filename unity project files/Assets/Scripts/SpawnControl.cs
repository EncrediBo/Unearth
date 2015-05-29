using UnityEngine;
using System.Collections;

public class SpawnControl : MonoBehaviour {

    //public SandpitDepthView sdv;
    private byte[] finalMap;

    //Prefabs of all the creatures/plants that can be spawned
    public GameObject seaMan;
    public GameObject landMan;

    public SandpitDepthView sdv;

    //Prefabs for trees
    public GameObject[] trees;  //Array of tree prefabs.
    public GameObject[] seaWeeds;	//Array of sea plants prefabs

    //All the data keeping track of the stuff that has spawned
    private int seaManCount = 0;
    private int seaManMax = 5555;
    private static bool seaManSpawn = true;

    private int landManCount = 0;
    private int landManMax = 5;
    private static bool landManSpawn = false;

    private int landPlantCount = 0;
    private int landPlantMax = 5555555;
    private static bool landPlantSpawn = true;

    private int seaPlantCount = 0;
    private int seaPlantMax = 5555555;
    private static bool seaPlantSpawn = true;

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
            int spawnLoc = sdv.getRandomSpawnLocation(1); //get spawn location in ushort
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
            int spawnLoc = sdv.getRandomSpawnLocation(3); //get spawn location in ushort
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

        if (seaPlantSpawn == true && seaPlantCount < seaPlantMax)
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
            Instantiate(seaWeeds[Random.Range(0, seaWeeds.Length)], new Vector3(mapX, mapY, 0f), Quaternion.identity);
            seaPlantCount++;
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
