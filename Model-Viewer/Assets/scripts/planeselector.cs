using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class planeselector : MonoBehaviour
{
    public GameObject intersectionQuad;
    [Range(0.5f, 0.0f)]
    public float speed;

    [SerializeField]
    private float maximumY = 43.9f;
    [SerializeField]
    private GameObject scancontroller;

    private float startY;
    private Vector3 startPos;
    private Quaternion defaultRotation;
    private Rigidbody m_Rigidbody;
    private bool movementEnabled = true;
    private Vector3 exactPosition;
    private int sliceCount =-1  ;
    private SliceRenderer sliceRenderer;

    public void removeCollision()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //Ignore the collisions between scan objects layer
        Physics.IgnoreLayerCollision(3, 3);
    }
    void Awake()
    {
        removeCollision();

        startPos = transform.localPosition;
        startY = transform.localPosition.y;
        defaultRotation = transform.rotation;
        exactPosition = startPos;
        sliceRenderer = scancontroller.GetComponent<scanController>().getSliceRenderer();
        if (sliceRenderer == null)
        {
            throw new ArgumentException();
        }
    }

    public void setSliceCount(int c)
    {
        sliceCount = c;
    }

    internal void hide()
    {
        movementEnabled = false;
        GetComponent<Renderer>().enabled = false;
    }
    /// <summary>
    /// reenable the plane selector, and update the rendered image 
    /// </summary>
    internal void show()
    {
        movementEnabled = true;
        GetComponent<Renderer>().enabled = true;
        renderImageSlice();
    }

    public void raiseLower(float d)
    {
        if (!movementEnabled)
        {
            return;
        }
        d = d * speed;
        exactPosition = new Vector3(exactPosition.x, exactPosition.y + d, exactPosition.z);
        //avoid out of bounds
        if (exactPosition.y < startY)
        {
            exactPosition = new Vector3(exactPosition.x, startY, exactPosition.z);
        }
        if (exactPosition.y > maximumY)
        {
            exactPosition = new Vector3(exactPosition.x, maximumY, exactPosition.z);
        }
        renderImageSlice();
    }
    /*
    send position to show a CT slice
    then set plane to match CT slice shown
    */
    private void renderImageSlice()
    {
        if (sliceRenderer == null || sliceCount == -1)
        {
            throw new ArgumentException();
        }
        float currentY = exactPosition.y;
        float exactPercent = (currentY - startY) / (maximumY - startY);
        int currentSlice = sliceRenderer.renderImage(exactPercent);
        if(currentSlice == -1){
            //nothing happened, dont move plane
            return;
        }
        print("current slice returned: " + currentSlice);
        float dicomHeight = (maximumY - startY);
        dicomHeight = Math.Abs(dicomHeight);
        float currentSlicePercentage = ((float)currentSlice / ((float)sliceCount - 1));
        float Y_offset = currentSlicePercentage * dicomHeight;
        float stepY = startY + Y_offset;
        print("start Y " + startY + " offset Y " + Y_offset + " height: " + dicomHeight);
        print("moves to stepY " + stepY);
        transform.localPosition = new Vector3(exactPosition.x, stepY, exactPosition.z);
        // print("moves plane to match slice " + currentSlice + " new Y is " + stepY);
    }

    internal void setIntersectionMode(bool isIntersectionEnabled)
    {
        if (isIntersectionEnabled)
        {
            GetComponent<Renderer>().enabled = false;
            intersectionQuad.GetComponent<Renderer>().enabled = true;
            throw new Exception("should not happen");
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
            intersectionQuad.GetComponent<Renderer>().enabled = false;
        }
    }

    internal void setTexture(Texture2D texture)
    {
        intersectionQuad.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }
}
