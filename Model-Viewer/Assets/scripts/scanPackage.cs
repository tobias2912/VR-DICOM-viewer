using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// a class representing a specific instance of a DICOM package
public class scanPackage : MonoBehaviour
{
    [Header("short identifying string used in folder name")]
    public string identifier;
    public string description;
    [Header("Dicom data")]
    public Texture2D[] filenames;
    public TextAsset[] bytes;
    public Texture2D[] images;

    [Header("other")]
    [SerializeField]
    private bool hidden = true;
    private scanObject[] scanObjects;
    private float hideDistance = 2f;
    [SerializeField]
    private bool HighlightOnHover;

    public String[] getFileNames()
    {
        List<String> names = new List<string>();
        foreach (var img in filenames)
        {
            names.Add(img.name);
        }
        return names.ToArray();
    }

    void Start()
    {
        scanObjects = GetComponentsInChildren<scanObject>();
        setHighlightOnHover(HighlightOnHover);

    }

    private void setHighlightOnHover(bool highlightOnHover)
    {
        foreach (scanObject ob in scanObjects)
        {
            ob.highlightOnHover = highlightOnHover;
        }
    }

    //store reference to child objects, because XR interactor changes parent when grabbing
    public scanObject[] getScanObjects()
    {
        return scanObjects;
    }
    public void hide()
    {
        if (hidden) return;
        hidden = true;
        print("hides " + gameObject.name);
        setRenderersColliders(false);
        foreach (var c in GetComponentsInChildren<NetworkSyncTransform>())
        {
            c.syncTransformToServer();
        }
    }

    private void setRenderersColliders(bool v)
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.enabled = v;
        }
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.enabled = v;
        }
    }

    public void show()
    {
        if (!hidden) return;
        hidden = false;
        // transform.Translate(Vector3.up * hideDistance);
        setRenderersColliders(true);
        foreach (var c in GetComponentsInChildren<NetworkSyncTransform>())
        {
            c.syncTransformToServer();
        }
    }
}
