using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
/*
class responsible for changing what scan is shown
and load new CT images for new models
and setting shader type
*/
public class scanController : MonoBehaviour
{
    [Header("use either preconfigured images or DICOM libary")]
    [SerializeField]
    private bool useRuntimeSliceRendering = true;
    [SerializeField]
    private GameObject sliceRendererStaticObj;
    [SerializeField]
    private GameObject sliceRendererDynamicObj;
    [SerializeField]
    private GameObject activeGameObject;
    [SerializeField]
    private int currentScanIndex;
    [SerializeField]
    private Shader normalShader;
    [SerializeField]
    private Shader cutoffShader;


    public scanCollection scanCollection;

    void Start()
    {
        if (useRuntimeSliceRendering)
        {
            sliceRendererStaticObj.SetActive(false);
        }
        else
        {
            sliceRendererDynamicObj.SetActive(false);
        }
        switchToScan(currentScanIndex);
    }

    public SliceRenderer getSliceRenderer()
    {
        SliceRenderer sliceRenderer = null;
        if (useRuntimeSliceRendering)
        {
            sliceRenderer = sliceRendererDynamicObj.GetComponent<SliceRenderer>();
        }
        else
        {
            sliceRenderer = sliceRendererStaticObj.GetComponent<SliceRenderer>();
        }
        if (sliceRenderer == null) throw new System.Exception("did not find slicerenderer");
        return sliceRenderer;

    }

    /*
    toggle between normal shader and shader creating a cutoff effect
    */
    internal void setIntersectionShader(bool cutoffEnabled)
    {
        foreach (scanObject obj in GetComponentsInChildren<scanObject>())
        {
            GenericThreePlanesCuttingController cut = obj.GetComponent<GenericThreePlanesCuttingController>();
            if(cut == null){
                Debug.LogWarning(obj.name+" does not have any intersection controller");
                return;
            }
            cut.cutoffEnabled = cutoffEnabled;
            Renderer rend = obj.GetComponent<Renderer>();
            if (cutoffEnabled)
            {
                rend.material.shader = cutoffShader;
            }
            else
            {
                rend.material.shader = normalShader;
            }
        }
    }

    public List<scanPackage> getScanPackages()
    {
        var list = new List<scanPackage>();
        foreach (var scan in GetComponentsInChildren<scanPackage>(true))
        {
            list.Add(scan);
        }
        return list;
    }

    public scanPackage getCurrentScanPackage()
    {
        scanPackage scanPackage = getScanPackages()[currentScanIndex];
        if (scanPackage == null)
        {
            throw new System.Exception("no scan was active");
        }
        return scanPackage;
    }

    public void switchToScan(int index)
    {
        print("changes to scan nr " + index);
        scanCollection.destroySpawnedObjs();
        scanCollection.resetScale();
        currentScanIndex = index;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (i == index)
            {
                child.GetComponent<scanPackage>().show();
                activeGameObject = child;
            }
            else
            {
                child.GetComponent<scanPackage>().hide();
            }
        }
        sliceRendererStaticObj.GetComponent<SliceRenderer>().changeActiveImages(getCurrentScanPackage());
    }

    public void nextScan()
    {
        int index = (currentScanIndex + 1) % 2;
        switchToScan(index);
    }

    public planeselector getPlaneSelector()
    {
        return activeGameObject.GetComponentInChildren<planeselector>();
    }
}
