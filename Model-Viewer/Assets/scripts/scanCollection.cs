using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

/*
util class for all objects related to the scan objects
also deals with all item rotation/scales etc to everything consistent
*/
public class scanCollection : NetworkBehaviour
{
    // private ArrayList colors;
    [Range(1.0f, 5.0f)]
    public float scaleSpeed;
    [Range(1.0f, 50.0f)]
    public float rotatespeed;
    [Range(0.001f, 0.1f)]
    public float distanceSpeed;
    public GameObject rotateAround;
    [SerializeField]
    private List<Color> colors;
    [SerializeField]
    private GameObject pupupText;
    [SerializeField]
    private GameObject scans;

    private GrabModeController modeController;
    private float currentRotateInput;
    new GameObject camera;

    void Start()
    {
        modeController = GameObject.Find("GrabModeController").GetComponent<GrabModeController>();
        addRandomColors();
        camera = GameObject.Find("Main Camera");
        scans.GetComponent<scanController>().scanCollection = this;
    }

    public void destroySpawnedObjs()
    {
        spawnable[] spawnables = GetComponentsInChildren<spawnable>();
        print("removing spawned objs " + spawnables.Length);
        foreach (var spawned in spawnables)
        {
            print("destroy " + spawned.name);
            Destroy(spawned.gameObject);
        }
    }
    public scanController GetScanController()
    {
        return scans.GetComponent<scanController>();
    }
    public void resetAllChildTransforms()
    {
        print("resetAllChildTransforms");
        modeController.CMDenableConnectedMode();
        foreach (var scan in GetComponentsInChildren<scanObject>())
        {
            print("was a scanobject, resets position");
            scan.resetPosition();
        }
    }
    /// <summary>
    /// set child gameobjects to different material colors
    /// </summary>
    private void addRandomColors()
    {
        int i = 0;
        foreach (var scan in GetComponentsInChildren<scanObject>())
        {
            Renderer rend = scan.gameObject.GetComponent<Renderer>();
            try
            {
                scan.setColor(colors[i]);
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.LogWarning("not enough colors to apply to model");
                scan.setColor(colors[0]);
            }
            i++;
        }
    }
    private Color from256(int r, int g, int b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    public void rotate(float d)
    {
        currentRotateInput = d;
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * d * rotatespeed);
        syncTransform(transform);
    }
    public void raisePlane(float d){
        GetScanController().getPlaneSelector().raiseLower(d);
    }

    ///<summary>    
    ///multiply the scale of all child gameobjects
    ///</summary>
    public void scale(float d)
    {
        //avoid scaling if also grabbing
        if (modeController.isCurrentlyGrabbing())
        {
            print("abort scaling");
            return;
        }
        //avoid scaling too small
        if (d < 0 && transform.localScale.x <= 0.1f)
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            return;
        }
        //avoid scaling when user is rotating or input very low
        if (Math.Abs(d) < Math.Abs(currentRotateInput) || Math.Abs(d) < 0.1f)
            return;
        Vector3 prev = transform.localScale;
        float newFloat = calculateScale(prev.x, d);
        Vector3 newScale = new Vector3(newFloat, newFloat, newFloat);
        transform.localScale = newScale;
        ///moves towards player to keep distance equal
        // transform.position = Vector3.MoveTowards(transform.position, camera.transform.position, distanceSpeed * (-d));
        syncTransform(transform);
        displayScaleText();
    }

    public void resetScale()
    {
        Vector3 newScale = new Vector3(0.6f, 0.6f, 0.6f);
        transform.localScale = newScale;
        syncTransform(transform);
    }

    private void displayScaleText()
    {
        float percentScale = transform.localScale.x * 1000;
        if (percentScale < 100f) percentScale = 100f;
        string t = "scaling: " + string.Format("{0,6:##0.}", percentScale) + "%";
        pupupText.GetComponent<pupupTextController>().showText(t, 0.8f);
    }

    // [Command(requiresAuthority = false)]
    private void syncTransform(Transform t)
    {
        this.transform.position = t.position;
        this.transform.rotation = t.rotation;
        this.transform.localScale = t.localScale;
        RpcSetPos(this.transform);
        foreach (var c in GetComponentsInChildren<NetworkSyncTransform>())
        {
            c.syncTransformToServer();
        }
    }

    [ClientRpc]
    void RpcSetPos(Transform transform)
    {

    }

    private float calculateScale(float prev, float joyInput)
    {
        float newScale = (float)(prev + joyInput * 0.001 * scaleSpeed);
        return newScale;

    }
    ///number from 0..infinite of how the model is scaled from default
    public float getModelScale()
    {
        //assumes start scale is 1,1,1
        Vector3 scale = transform.localScale;
        if (scale.x != scale.y || scale.y != scale.z)
        {
            throw new System.Exception("not uniform scale");
        }
        float factor = scale.x;
        return factor;
    }
}
