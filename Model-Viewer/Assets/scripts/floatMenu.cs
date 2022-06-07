using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class floatMenu : MonoBehaviour
{
    public new GameObject camera;
    [Range(0.1f, 2.0f)]
    public float UIdistance;
    public bool isVisible;
    private float startY;
    public GameObject scanCollection;


    void Start()
    {
        gameObject.SetActive(isVisible);
        startY = transform.position.y;
        addOutline();
    }

    private void addOutline()
    {
        Image image = GetComponentInChildren<Image>();
        NicerOutline nicerOutline = image.gameObject.AddComponent<NicerOutline>();
        nicerOutline.effectColor = new Color(0,0,0,1);
    }

    public void toggleVisibility()
    {
        print("menu visibility to" + !isVisible);
        if (isVisible)
        {
            hide();
        }
        else
        {
            show();
        }

    }

    public void show()
    {
        gameObject.SetActive(true);
        scanCollection.SetActive(false);
        Vector3 temp = camera.transform.position + camera.transform.forward * UIdistance;
        // temp.y=startY;
        transform.position = temp;
        transform.rotation = Quaternion.LookRotation(transform.position - camera.transform.position);
        isVisible = true;
    }

    public void hide()
    {
        gameObject.SetActive(false);
        scanCollection.SetActive(true);
        isVisible = false;
    }
}
