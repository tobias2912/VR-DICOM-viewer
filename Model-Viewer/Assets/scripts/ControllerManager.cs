using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ControllerManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject rightServer;
    [SerializeField]
    private GameObject leftServer;

    private GameObject leftLocal;    
    private GameObject rightLocal;    

    void Start()
    {
        if (!isLocalPlayer) return;
        rightLocal = GameObject.Find("RightHand Controller");
        leftLocal = GameObject.Find("LeftHand Controller");
        //hide player owns controller copy
        rightServer.GetComponent<MeshRenderer>().enabled = false;
        leftServer.GetComponent<MeshRenderer>().enabled = false;
        if (rightLocal == null || leftLocal == null)
        {
            throw new System.Exception("did not find a controller"+rightLocal+leftLocal);
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        //make the synced controller follow the player actual controllers
        if (isServer)
        {
            Move(leftServer, leftLocal.transform.position, leftLocal.transform.rotation);
            Move(rightServer, rightLocal.transform.position, rightLocal.transform.rotation);
        }
        else
        {
            Move(leftServer, leftLocal.transform.position, leftLocal.transform.rotation);
            Move(rightServer, rightLocal.transform.position, rightLocal.transform.rotation);
        }
    }

//nut used???
    [Command]
    void CmdMove(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
    void Move(GameObject controller, Vector3 pos, Quaternion rot)
    {
        controller.transform.position = pos;
        controller.transform.rotation = rot;
    }
}
