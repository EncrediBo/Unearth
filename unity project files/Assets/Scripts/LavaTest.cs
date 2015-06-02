using UnityEngine;
using System.Collections;

public class LavaTest : MonoBehaviour {

	private int lavaX;
	private int lavaY;
	private int[] lavaHeatMap;
	private bool check;

	public PathFinder pf;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lavaHeatMap = pf.getHeatMap (3);

	
	}
}
