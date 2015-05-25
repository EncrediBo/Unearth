using UnityEngine;
using System.Collections;

public class SpawnControl : MonoBehaviour {

    //public SandpitDepthView sdv;
    private byte[] finalMap;

    //Prefabs of all the creatures/plants that can be spawned
    public GameObject seaMan;
    public GameObject landMan;


    //All the data keeping track of the stuff that has spawned
    private int seaManCount = 0;
    private int seaManMax = 5;
    private int landManCount = 0;
    private int landManMax = 5;
    private static bool seaManSpawn = false;
    private static bool landManSpawn = false;

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
        Instantiate(seaMan);
        Instantiate(landMan);
        _instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        //finalMap = sdv.getMap();
        //Debug.Log("seamancount smaller than seamanmax: "+ (seaManCount < seaManMax));

        if (seaManSpawn == true && seaManCount < seaManMax)
        {
            Debug.Log(seaManCount);
            Debug.Log(seaManMax);
            Instantiate(seaMan, new Vector3(-1.36793f, -1.36793f, 0f), Quaternion.identity);
            seaManCount++;
            seaManSpawn = false;
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
