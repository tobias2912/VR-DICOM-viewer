using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/*
controller with utility for handling grab modes and what parts are grabbed
*/
public class GrabModeController : NetworkBehaviour
{
    // makes sure if parts are moved around, measurements and CT scans 
    // are not shown so its not misleading. Reverting to default positions shows
    // them again 
    public GameObject sliceCanvas;
    public GameObject scanCollectionObj;
    public GameObject measure;
    public GameObject anglemeasure;

    private SliceRenderer SliceRenderer;
    private scanCollection scanCollection;
    private planeselector planeselector;
    private MeasurePlacement measurePlacement;
    private AngleMeasurePlacement anglemeasurePlacement;
    private bool separationMode = false;

    void Start()
    {
        scanCollection = scanCollectionObj.GetComponent<scanCollection>();
        SliceRenderer = scanCollection.GetScanController().getSliceRenderer();
        planeselector = scanCollection.GetScanController().getPlaneSelector();
        measurePlacement = measure.GetComponent<MeasurePlacement>();
        anglemeasurePlacement = anglemeasure.GetComponent<AngleMeasurePlacement>();
    }
    
    //true if any of the grabbables are currently grabbed
    public bool isCurrentlyGrabbing()
    {
        scanPackage scanPackage = scanCollection.GetScanController().getCurrentScanPackage();
        foreach (var obj in scanPackage.getScanObjects())
        {
            offsetGrab offsetgrab = obj.GetComponent<offsetGrab>();
            if (offsetgrab.currentlyGrabbed)
            {
                return true;
            }
        }
        return false;
    }

    [Command(requiresAuthority = false)]
    public void CMDenableSeparationMode()
    {
        enableSeparationMode();
    }

    [ClientRpc]
    public void enableSeparationMode()
    {
        print("separation mode");
        separationMode = true;
        SliceRenderer.hideImage();
        planeselector.hide();
        measurePlacement.hide();
        anglemeasurePlacement.hide();
    }

    [Command(requiresAuthority = false)]
    public void CMDenableConnectedMode()
    {
        RPCenableConnectedMode();
    }
    public bool isSeparationMode()
    {
        return separationMode;
    }
    [ClientRpc]
    public void RPCenableConnectedMode()
    {
        print("connected mode");
        separationMode = false;
        SliceRenderer.showImage();
        planeselector.show();
        measurePlacement.show();
        anglemeasurePlacement.show();
    }

    //true if the specific interactable is currently grabbed
    internal bool isCurrentlyGrabbing(XRBaseInteractable interactable)
    {
        scanPackage scanPackage = scanCollection.GetScanController().getCurrentScanPackage();
        foreach (var obj in scanPackage.getScanObjects())
        {
            offsetGrab offsetgrab = obj.GetComponent<offsetGrab>();
            if (offsetgrab.currentlyGrabbed)
            {
                if (offsetgrab.Equals(interactable))
                {
                    return true;
                }
                //return false her?
            }
        }
        return false;
    }
}
