using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliceRendererDynamic : MonoBehaviour, SliceRenderer
{
    public GameObject imageobject;
    public GameObject text;
    private quadscript slices;
    private scanController controller;
    private planeselector selector;
    [SerializeField]
    private GameObject scanCollection;
    [SerializeField]
    private GameObject descriptionObj;
    private float lastPercentInput;
    public bool isIntersectionEnabled;

    void Start()
    {
        slices = imageobject.GetComponent<quadscript>();
        controller = scanCollection.GetComponent<scanCollection>().GetScanController();
        changeActiveImages(controller.getCurrentScanPackage());
        setRenderIntersection(isIntersectionEnabled);
    }
    public void setRenderIntersection(bool v)
    {
        isIntersectionEnabled = v;
        print("show intersection slice: "+isIntersectionEnabled);
        selector.setIntersectionMode(isIntersectionEnabled);
        controller.setIntersectionShader(isIntersectionEnabled);
    }
    public void toggleRenderIntersection()
    {
        setRenderIntersection(!isIntersectionEnabled);
    }
    /*
    change what dicom data should be used
    */
    public void changeActiveImages(scanPackage newScan)
    {
        selector = controller.getPlaneSelector();
        descriptionObj.GetComponent<TextMeshProUGUI>().text = newScan.description;
        bool success = slices.loadDicomData(newScan.identifier, newScan);
        if (success)
        {
            selector.setSliceCount(slices.sliceCount());
            selector.show();
        }
        else
        {
            print("hides plane selector");
            descriptionObj.GetComponent<TextMeshProUGUI>().text += "\n(CT images missing)";
            selector.hide();
        }
    }

    public void hideImage()
    {
        imageobject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void showImage()
    {
        imageobject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void displayInfo(string v)
    {
        TextMeshProUGUI textMesh = text.GetComponent<TextMeshProUGUI>();
        textMesh.text = v;
    }

    public int renderImage(float percent)
    {
        if(!slices.isReady())
        {
            print("slices not ready");
            return 0;
        }
        if (percent >= 100f) percent = 0.99f;
        if (!enabled)
        {
            print("render image was inactive");
            return -1;
        }
        //37 bilder burde gi [0..36]
        int imagenum = (int)(slices.sliceCount() * percent);
        if (imagenum >= slices.sliceCount()) imagenum = slices.sliceCount() - 1;
        int slicesFromTop = slices.sliceCount() - imagenum - 1;
        // print(percent+"% imagenum calculated to " + imagenum);
        showImageInfo(percent, imagenum);
        renderImageDelayed(percent, slicesFromTop);
        return imagenum;
    }

    private void renderImageDelayed(float percent, int imagenum)
    {
        lastPercentInput = percent;
        StartCoroutine(waiter(percent, imagenum));
    }
    IEnumerator waiter(float currentPercent, int imagenum)
    {
        yield return new WaitForSeconds(0.05f);
        if (currentPercent == lastPercentInput)
        {
            //has been standing still for 0.2 seconds
            render(imagenum);
        }
    }
    private void showImageInfo(float percent, int imagenum)
    {
        TextMeshProUGUI textMesh = text.GetComponent<TextMeshProUGUI>();
        String percentText = Math.Round(percent * 100).ToString();
        textMesh.text = " " + percentText + " %\n image " + (imagenum + 1) + " / " + slices.sliceCount();
    }

    /*
    render slice to screen and optionally also to the plane
    */
    private void render(int number)
    {
        if (number >= slices.sliceCount() || number < 0)
        {
            print("render " + number + " doesnt exist");
            return;
        }
        Texture2D texture = slices.showSlice(number);
        selector.setIntersectionMode(isIntersectionEnabled);
        if (isIntersectionEnabled)
        {
            selector.setTexture(texture);
        }
    }
}
