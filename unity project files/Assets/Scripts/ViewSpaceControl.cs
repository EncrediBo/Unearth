using UnityEngine;
using System.Collections;

public class ViewSpaceControl : MonoBehaviour {
    public SandpitDepthView spd;

    private float viewScale;
    private float xPos;
    private float yPos;

    private bool redraw = true;

	// Use this for initialization
	void Start () {
        viewScale = 141;
        xPos = 184;
        yPos = 156;

        spd = GetComponent<SandpitDepthView>();
	}
	
	// Update is called once per frame
	void Update () {
        spd.viewScale = (int)viewScale;
        spd.viewX = (int)xPos;
        spd.viewY = (int)yPos;

        if (redraw)
        {
            redraw = false;
            spd.DrawMaskingCircle();
        }
	}

    public void zoomIn()
    {
        viewScale -= 1f;
        redraw = true;
    }

    public void zoomOut()
    {
        viewScale += 1f;
        redraw = true;
    }

    public void moveUp()
    {
        yPos -= 1f;
        redraw = true;
    }

    public void moveDown()
    {
        yPos += 1f;
        redraw = true;
    }

    public void moveLeft()
    {
        xPos += 1f;
        redraw = true;
    }

    public void moveRight()
    {
        xPos -= 1f;
        redraw = true;
    }
}
