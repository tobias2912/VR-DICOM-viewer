using System;
using TMPro;
using UnityEngine;

public class MeasurePlacement : MonoBehaviour, Tool
{
    public GameObject start;
    public GameObject end;
    public GameObject lineRendererObj;
    public GameObject rightController;
    public GameObject leftController;
    public GameObject text;
    public GameObject scancollectionobj;

    private Vector3 startPos;
    private float currentDistance;
    private AudioSource audioSource;
    private TextMeshProUGUI textMesh;
    private scanCollection scancollection;
    private LineRenderer linerenderer;
    private bool placementOngoing = false;
    private GameObject activeController;

    void Start()
    {
        activeController = rightController;
        scancollection = scancollectionobj.GetComponent<scanCollection>();
        linerenderer = lineRendererObj.GetComponent<LineRenderer>();
        startPos = new Vector3();
        // Invoke("addListener", 2);
        textMesh = this.text.GetComponent<TextMeshProUGUI>();
        audioSource = end.GetComponent<AudioSource>();
    }
    void Update()
    {
        Vector3 controllerPos = activeController.transform.position;
        if (placementOngoing)
        {
            end.transform.position = controllerPos;
            linerenderer.SetPosition(0, start.transform.position);
            linerenderer.SetPosition(1, controllerPos);
            textMesh.gameObject.transform.position = controllerPos;
            textMesh.gameObject.transform.position = (controllerPos + start.transform.position) / 2;
            float dist = distance(start.transform.position, controllerPos);
            if (Math.Abs(currentDistance - dist) > 0.35f)
            {
                audioSource.Play();
                currentDistance = dist;
            }
            updateDistanceText(dist);
        }
        else
        {
            //update line in case start and end points have moved
            linerenderer.SetPosition(0, start.transform.position);
            linerenderer.SetPosition(1, end.transform.position);
            textMesh.gameObject.transform.position = (end.transform.position + start.transform.position) / 2;
        }
    }

    internal void show()
    {
        linerenderer.enabled = true;
        start.SetActive(true);
        end.SetActive(true);
        textMesh.gameObject.SetActive(true);
    }

    internal void hide()
    {
        linerenderer.enabled = false;
        start.SetActive(false);
        end.SetActive(false);
        textMesh.gameObject.SetActive(false);
    }

    public void placePoint(bool isRightController)
    {
        // print("placepoint. placeing last point " + placementOngoing);
        Vector3 controllerPos;
        if (isRightController)
        {
            activeController = rightController;
        }
        else
        {
            activeController = leftController;
        }
        controllerPos = activeController.transform.position;
        if (placementOngoing)
        {
            //placed a line
            placementOngoing = false;
            end.transform.position = controllerPos;
            linerenderer.SetPosition(0, start.transform.position);
            linerenderer.SetPosition(1, end.transform.position);
            float dist = distance(start.transform.position, end.transform.position);
            updateDistanceText(dist);
        }
        else
        {
            //begin new measure
            end.transform.position = new Vector3(0f, 0f, 0f);
            startPos = controllerPos;
            start.transform.position = controllerPos;
            placementOngoing = true;
        }
    }
    //distance in mm
    private float distance(Vector3 start, Vector3 end)
    {
        Vector3 vector3 = start - end;
        float dist = vector3.magnitude * 100;
        return dist;
    }
    private void updateDistanceText(float dist)
    {
        dist = dist / scancollection.getModelScale();
        String text = Math.Round(dist, 1).ToString("N1") + " mm";
        textMesh.text = text;
    }

    public void ToolSelected(Tooltype type)
    {
        
    }

    public void ToolActivated(bool isRightController)
    {
        placePoint(isRightController);
    }

    public void ToolDeselected()
    {
        if(placementOngoing){
            placePoint(true);
        }
    }
}