using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class autostartServerHost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NetworkManager>().StartHost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
