using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ToolController;
using Mirror;

public partial class PlaceImplant :NetworkBehaviour, Tool
{
    public GameObject screwprefab;
    public GameObject plateprefab;
    public GameObject scanCollectionOb;
    private scanCollection scanCollection;
    [SerializeField]
    private GameObject grabModeController;

    void Start()
    {
        scanCollection = scanCollectionOb.GetComponent<scanCollection>();
    }

    [Command(requiresAuthority = false)]
    public void createimplant(Tooltype type)
    {
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 0.4f;
        GameObject screw = null;
        if (type == Tooltype.Screw50mm)
        {
            screw = Instantiate(screwprefab, pos, new Quaternion(0f,90f,90f,0f));
        }
        else if (type == Tooltype.Screw80mm)
        {
            screw = Instantiate(screwprefab, pos, new Quaternion(0f,90f,90f,0f));
            screw.transform.localScale*=(0.8f/0.5f);
        }
        else if (type == Tooltype.Plate)
        {
            screw = Instantiate(plateprefab, pos, new Quaternion(0f,90f,90f,0f));
            screw.transform.localScale*=(0.25f);
        }
        else{
            throw new System.Exception("no type found");
        }
        NetworkServer.Spawn(screw);
        screw.GetComponent<offsetGrab>().modeController = grabModeController;
        screw.transform.SetParent(scanCollection.transform);
        screw.transform.localScale = screw.transform.localScale * scanCollection.getModelScale();

    }

    public void ToolSelected(Tooltype type)
    {
        createimplant(type);
    }

    public void ToolActivated(bool isRightController)
    {
        //what should this do
    }

    public void ToolDeselected()
    {
        
    }
}
