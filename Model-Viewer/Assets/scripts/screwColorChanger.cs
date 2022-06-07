using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screwColorChanger : MonoBehaviour
{
    private Outline outline;
    private Color original;
    public Color TempColor;

    void Start()
    {

        outline = gameObject.GetComponent<Outline>();
        original = outline.OutlineColor;
    }

    public void hoverEnter()
    {
        outline.OutlineColor = TempColor;
    }
    public void hoverExit()
    {

        outline.OutlineColor = original;
    }

}
