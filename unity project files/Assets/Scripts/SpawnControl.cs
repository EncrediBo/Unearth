using UnityEngine;
using System.Collections;

public class SpawnControl : MonoBehaviour {

    //public SandpitDepthView sdv;
    private byte[] finalMap;

    //Prefabs of all the creatures/plants that can be spawned
    public GameObject[] seaMan;
    public GameObject[] landMan;

    public SandpitDepthView sdv;

    //Prefabs for plants
    public GameObject[] landPlants;  //Array of tree prefabs.
    public GameObject[] seaPlants;	//Array of sea plants prefabs
    public GameObject volcano;

    //All the data keeping track of the stuff that has spawned
    private int seaManCount = 0;
    private int seaManMax = 15;
    private static bool seaManSpawn = true;

    private int landManCount = 0;
    private int landManMax = 15;
    private static bool landManSpawn = true;

    private int landPlantCount = 0;
    private int landPlantMax = 100;
    private static bool landPlantSpawn = true;

    private int seaPlantCount = 0;
    private int seaPlantMax = 5;
    private static bool seaPlantSpawn = true;

    private int landKillerCount = 0;
    private int landKillerMax = 1;
    private static bool landKillerSpawn = true;

    private int seaKillerCount = 0;
    private int seaKillerMax = 1;
    private static bool seaKillerSpawn = true;

    //Volcano
    private static bool volcanoSpawn = true;

    protected const float pixelHeight = 0.023585f;

    //Initial spawn delay in seconds, to wait for the kinect
    private float delay = 2f; //Every two seconds

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
        _instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
            return;
        }
        //finalMap = sdv.getMap();
        //Debug.Log("seamancount smaller than seamanmax: "+ (seaManCount < seaManMax));
        if (landManSpawn == true && landManCount < landManMax)
        {
            int spawnLoc = sdv.getRandomSpawnLocation(3); //get spawn location in ushort
            //convert to x and y values
            int x = spawnLoc % 424;
            int y = spawnLoc / 424;
            //convert to canvas values
            float mapX = (212 - x) * pixelHeight;
            float mapY = (212 - y) * pixelHeight;

            //Debug.Log(landManCount);
            //Debug.Log(landManMax);
            Instantiate(landMan[Random.Range(0, landMan.Length)], new Vector3(mapX, mapY, 0f), Quaternion.identity);
            landManCount++;
            //seaManSpawn = false;
        }

        if (seaManSpawn == true && seaManCount < seaManMax)
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
            Instantiate(seaMan[Random.Range(0, seaMan.Length)], new Vector3(mapX, mapY, 0f), Quaternion.identity);
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
            Instantiate(landPlants[Random.Range(0, landPlants.Length)], new Vector3(mapX, mapY, 0f), Quaternion.identity);
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
            Instantiate(seaPlants[Random.Range(0, seaPlants.Length)], new Vector3(mapX, mapY, 0f), Quaternion.identity);
            seaPlantCount++;
            //seaManSpawn = false;
        }

        if (volcanoSpawn == true)
        {
            int spawnLoc = sdv.getRandomSpawnLocation(6); //get spawn location in ushort
            //convert to x and y values
            int x = spawnLoc % 424;
            int y = spawnLoc / 424;
            //convert to canvas values
            float mapX = (212 - x) * pixelHeight;
            float mapY = (212 - y) * pixelHeight;

            //Debug.Log(seaManCount);
            //Debug.Log(seaManMax);
            Instantiate(volcano, new Vector3(mapX, mapY, 0f), Quaternion.identity);
            volcanoSpawn = false;
            //seaManSpawn = false;
        }
        //Instantiate(seaMan);
	}


    public void Spawn(int type)
    {
        switch (type)
        {
            case -1:
                landManCount--;
                break;
            case 1:
                landManCount++;
                break;
            case -2:
                seaManCount--;
                break;
            case 2:
                seaManCount++;
                break;
            case -3:
                landPlantCount--;
                break;
            case 3:
                landPlantCount++;
                break;
            case -4:
                seaPlantCount--;
                break;
            case 4:
                seaPlantCount++;
                break;
            case -5:
                landKillerCount--;
                break;
            case 5:
                landKillerCount++;
                break;
            case -6:
                seaKillerCount--;
                break;
            case 6:
                seaKillerCount++;
                break;
            default:
                break;
        }
    }

    

}
