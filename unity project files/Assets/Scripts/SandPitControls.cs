using UnityEngine;
using System.Collections;

public class SandPitControls : MonoBehaviour {

    Renderer renderer;

    private float offsetX = 0.31f;
    private float offsetY = 0.13f;

    private float scaleX = 0.5500004f;
    private float scaleY = 0.5600004f;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void minusOffSetX()
    {
        offsetX -= 0.01f;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }

    public void plusOffSetX()
    {
        offsetX += 0.01f;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }

    
    public void minusOffsetY()
    {
        offsetY -= 0.01f;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }

    public void plusOffSetY()
    {
        offsetY += 0.01f;
        renderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
    
    public void minusScaleX()
    {
        scaleX -= 0.01f;
        renderer.material.mainTextureScale = new Vector2 (scaleX, scaleY);
    }

    public void plusScaleX()
    {
        scaleX += 0.01f;
        renderer.material.mainTextureScale = new Vector2 (scaleX, scaleY);
    }

    public void minusScaleY()
    {
        scaleY -= 0.01f;
        renderer.material.mainTextureScale = new Vector2 (scaleX, scaleY);
    }

    public void plusScaleY()
    {
        scaleY += 0.01f;
        renderer.material.mainTextureScale = new Vector2 (scaleX, scaleY);
    }
}
