using UnityEngine;
using System.Collections;

public class ViewSpaceControl : MonoBehaviour {
    public SandpitDepthView spd;

    private float viewScale;
    private float xPos;
    private float yPos;

	// Use this for initialization
	void Start () {
        viewScale = 163;
        xPos = 184;
        yPos = 163;

        spd = GetComponent<SandpitDepthView>();
	}
	
	// Update is called once per frame
	void Update () {
        spd.viewScale = (int)viewScale;
        spd.viewX = (int)xPos;
        spd.viewY = (int)yPos;
	}

    public void zoomIn()
    {
        viewScale -= 1f;
    }

    public void zoomOut()
    {
        viewScale += 1f;
    }

    public void moveUp()
    {
        yPos -= 1f;
    }

    public void moveDown()
    {
        yPos += 1f;
    }

    public void moveLeft()
    {
        xPos += 1f;
    }

    public void moveRight()
    {
        xPos -= 1f;
    }
}
