using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public Camera mainCamera;

    private float viewScale;
    private float xPos;
    private float yPos;

	// Use this for initialization
	void Start () {
        xPos = -0.1f;
        yPos = 1.2f;
        viewScale = 3.8f;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(xPos);
        //Debug.Log(yPos);
        //Debug.Log(viewScale);
        mainCamera.orthographicSize = viewScale;
        mainCamera.transform.position = new Vector3(xPos, yPos, -10);
	}

    public void zoomIn()
    {
        viewScale -= 0.1f;
    }

    public void zoomOut()
    {
        viewScale += 0.1f;
    }

    public void moveUp()
    {
        yPos -= 0.1f;
    }

    public void moveDown()
    {
        yPos += 0.1f;
    }

    public void moveLeft()
    {
        xPos += 0.1f;
    }

    public void moveRight()
    {
        xPos -= 0.1f;
    }

}
