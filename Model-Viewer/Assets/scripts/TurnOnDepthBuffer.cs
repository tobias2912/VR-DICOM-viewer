using UnityEngine;
using System.Collections;
 
public class TurnOnDepthBuffer : MonoBehaviour {
 
    // Use this for initialization
    void Start ()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}