using UnityEngine;
using System.Collections;
using System.IO;
using System;


public class ArrayToFile : MonoBehaviour {

	// Use this for initialization

    public DepthSourceManager dsm;
    public ushort[] depth;

	//Change ME to make it work on a new machine!!!
	private const string path = "C:\\Users\\Studio\\Documents\\Unearth\\ushort.txt"; 

	public void Test()
	{
        ushort[] depth = dsm.GetData();
		Debug.Log (path);
        
		using (FileStream fs = new FileStream(path, FileMode.Truncate, FileAccess.Write))
		{
            using (StreamWriter sw = new StreamWriter(fs))
			{
				foreach (ushort value in depth)
				{
					sw.Write(Convert.ToString(value) + ",");
				}
			}
		}

   
	

	}



    public ushort[] GetData()
    {
        depth = new ushort[512 * 424];
        string text = System.IO.File.ReadAllText(path);
        string[] lineValues = text.Split(',');
        //Debug.Log(lineValues.Length);
        for (int i = 0; i < lineValues.Length - 1; i++)
        {
            //Debug.Log(lineValues[i]);
            try
            {
                depth[i] = ushort.Parse(lineValues[i]);
            }
            catch (Exception ex)
            {
                Debug.Log((i - 1) + ": " + lineValues[i - 1]);
                Debug.Log(i + ": " + lineValues[i]);
                break; //throw new Exception();
            }
            // depth[i] = Convert.ToUInt16(lineValues[i]);
        }
            return depth;
    }



}

	

