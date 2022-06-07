using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliceRendererImages : MonoBehaviour, SliceRenderer
{
    public GameObject imageobject;
    public GameObject text;
    private List<Sprite> sprites = new List<Sprite>();
    private scanController controller;
    private planeselector selector;
    [SerializeField]
    private GameObject scanCollection;
    public Texture2D[] currentImages;
    private int currentRenderNum;
    [SerializeField]
    private int renderEveryN = 6;

    void Start()
    {
        controller = scanCollection.GetComponent<scanCollection>().GetScanController();
        selector = controller.getPlaneSelector();
        changeActiveImages(controller.getCurrentScanPackage());
    }
    /*
    update all references when the selected scan changes
    */
    public void changeActiveImages(scanPackage newScan)
    {
        selector = controller.getPlaneSelector();
        currentImages = newScan.images;
        createImages();
        //renderImage(0.1f);//start with bottom slice
    }

    private void createImages()
    {
        sprites.Clear();
        foreach (Texture2D tex in currentImages)
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprites.Add(sprite);
        }
        print("sets image count to " + sprites.Count);
        selector.setSliceCount(sprites.Count);
    }

    public void hideImage()
    {
        imageobject.GetComponent<Image>().enabled = false;
        // displayInfo("Reset position to show CR slices");
    }

    public void displayInfo(string v)
    {
        TextMeshProUGUI textMesh = text.GetComponent<TextMeshProUGUI>();
        textMesh.text = v;
    }

    public void showImage()
    {
        imageobject.GetComponent<Image>().enabled = true;
    }

    public int renderImage(float percent)
    {
        if (!enabled)
        {
            print("render image was inactive");
            return -1;
        }
        if (sprites.Count == 0)
        {
            print("no images to show");
            return -1;
        }
        int imagenum = (int)(sprites.Count * percent);
        if(imagenum % renderEveryN != 0){
            return -1;
        }
        bool success = render(sprites.Count - imagenum);
        if(!success){
            return -1;
        }
        showImageInfo(percent, imagenum);
        return imagenum;

    }
    private void showImageInfo(float percent, int imagenum)
    {
        TextMeshProUGUI textMesh = text.GetComponent<TextMeshProUGUI>();
        String percentText = Math.Round(percent * 100).ToString();
        textMesh.text = " " + percentText + " %\n image " + (imagenum + 1) + " / " + sprites.Count;
    }

    private bool render(int number)
    {
        if (number == currentRenderNum)
        {
            return false;
        }
        if (number >= currentImages.Length)
        {
            print(number + " higher than total of " + currentImages.Length + ", cancel render");
            return false;
        }
        print("render number " + number);
        imageobject.GetComponent<Image>().sprite = getSprite(number);
        return true;
    }

    private Sprite getSprite(int number)
    {
        return sprites[number];
    }

}
