using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class DepthSourceViewTest : MonoBehaviour
{
    /*public GameObject DepthSourceManager;
    private DepthSourceManager _DepthManager;

    //how far is the kinetic from the bottom of the sandpit
    private const int cameraDistance = 1000;

    //Input from the kinetic sensor as a ushort
    private ushort[] depthData;

    //Kinect depth resolution
    private const int screenLength = 512;
    private const int screenHeight = 424;

    //Array of colours post conversion from ushort
    

    void Start()
    {
        
    }

    
    
    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.TextField(new Rect(Screen.width - 250 , 10, 250, 20), "DepthMode: Depth to color test" + colourMap /*ViewMode.ToString()*///);
     /*   GUI.EndGroup();
    }

    void Update()
    {



        if (DepthSourceManager == null)
        {
            return;
        }

        _DepthManager = DepthSourceManager.GetComponent<DepthSourceManager>();
        if (_DepthManager == null)
        {
            return;
        }

        byte[] colourMap = byte[screenLength*screenHight*4];

        private void Convert(){
            depthData = _DepthManager.GetData();
            for (int i = 0; i > 512*424; i++){
             colourMap[i] = 10;
            }
        }

    }

    
    







    private void RefreshData(ushort[] depthData, int colorWidth, int colorHeight)
    {
    }
    
 */
}
