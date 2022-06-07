using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Base class for all items that are spawned and then moved around by player
*/
[RequireComponent(typeof(offsetGrab))]
[RequireComponent(typeof(NetworkSyncTransform))]
[RequireComponent(typeof(Collider))]
public class spawnable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
