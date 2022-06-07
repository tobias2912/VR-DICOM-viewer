
using System;
using TMPro;
using UnityEngine;
public enum State
{
    FIRST,
    MIDDLE,
    LAST

}
public static class Extensions
{
    public static State NextState(this State s)
    {
        State? next = null;
        if (s == State.FIRST)
        {
            next = State.MIDDLE;
            Debug.Log("next: " + next);
        }
        if (s == State.MIDDLE)
        {
            next = State.LAST;
        }
        if (s == State.LAST)
        {
            next = State.FIRST;
        }
        if (next == null)
        {
            throw new Exception("wrong state");
        }
        return (State)next;
    }
}
public class AngleMeasurePlacement : MonoBehaviour, Tool
{
    public GameObject start;
    public GameObject middle;
    public GameObject end;
    public GameObject lineRendererObj;
    public GameObject rightController;
    public GameObject leftController;
    public GameObject text;
    public GameObject scancollectionobj;
    public float textDistance;

    private Vector3 startPos;
    private float currentAngle;
    private AudioSource audioSource;
    private TextMeshProUGUI textMesh;
    private scanCollection scancollection;
    private LineRenderer linerenderer;
    private State placementState;
    private GameObject activeController;

    void Start()
    {
        placementState = State.FIRST;
        activeController = rightController;
        scancollection = scancollectionobj.GetComponent<scanCollection>();
        linerenderer = lineRendererObj.GetComponent<LineRenderer>();
        startPos = new Vector3();
        textMesh = this.text.GetComponent<TextMeshProUGUI>();
        audioSource = end.GetComponent<AudioSource>();
    }
    void Update()
    {
        Vector3 controllerPos = activeController.transform.position;
        if (placementState == State.MIDDLE)
        {
            //currently placing middle point
            middle.transform.position = controllerPos;
            linerenderer.SetPosition(0, start.transform.position);
            linerenderer.SetPosition(1, controllerPos);
            linerenderer.SetPosition(2, controllerPos); //hide last line segment
        }
        if (placementState == State.LAST)
        {
            //currently placing last point
            end.transform.position = controllerPos;
            linerenderer.SetPosition(0, start.transform.position);
            linerenderer.SetPosition(1, middle.transform.position);
            linerenderer.SetPosition(2, controllerPos);
            textMesh.gameObject.transform.position = (middle.transform.position + (middle.transform.position - end.transform.position).normalized * textDistance);
            //add angle preview when placing last point
            float angle = this.angle(start.transform.position, middle.transform.position, controllerPos);
            if (Math.Abs(currentAngle - angle) > 0.35f)
            {
                audioSource.Play();
                currentAngle = angle;
            }
            updateAngleText(angle);
        }
        if (placementState == State.FIRST)
        {
            //update line in case start and end points have moved
            linerenderer.SetPosition(0, start.transform.position);
            linerenderer.SetPosition(1, middle.transform.position);
            linerenderer.SetPosition(2, end.transform.position);
            textMesh.gameObject.transform.position = (middle.transform.position + (middle.transform.position - end.transform.position).normalized * textDistance);
        }
    }

    internal void show()
    {
        linerenderer.enabled = true;
        start.SetActive(true);
        middle.SetActive(true);
        end.SetActive(true);
        textMesh.gameObject.SetActive(true);
    }

    internal void hide()
    {
        linerenderer.enabled = false;
        start.SetActive(false);
        middle.SetActive(false);
        end.SetActive(false);
        textMesh.gameObject.SetActive(false);
    }

    public void placePoint(bool isRightController)
    {
        print("place angle. " + placementState);
        Vector3 controllerPos;
        activeController = isRightController ? rightController : leftController;
        controllerPos = activeController.transform.position;

        if (placementState == State.FIRST)
        {
            //begin new measure
            end.transform.position = new Vector3(0f, 0f, 0f);
            middle.transform.position = new Vector3(0f, 0f, 0f);
            startPos = controllerPos;
            start.transform.position = controllerPos;
        }
        if (placementState == State.MIDDLE)
        {
            //placed first line segment
            middle.transform.position = controllerPos;
            linerenderer.enabled = true; //test
            linerenderer.SetPosition(0, start.transform.position);
            linerenderer.SetPosition(1, middle.transform.position);
        }
        if (placementState == State.LAST)
        {
            end.transform.position = controllerPos;
            linerenderer.SetPosition(2, middle.transform.position);
            float angle = this.angle(start.transform.position, middle.transform.position, end.transform.position);
            updateAngleText(angle);
        }
        placementState = placementState.NextState();
    }
    private float angle(Vector3 start, Vector3 middle, Vector3 end)
    {
        return Vector3.Angle(start - middle, end - middle);

    }
    private void updateAngleText(float angle)
    {
        String text = Math.Round(angle, 0).ToString("N0") + "°";
        textMesh.text = text;
    }


    public void ToolActivated(bool isRightController)
    {
        placePoint(isRightController);
    }

    public void ToolSelected(Tooltype type)
    {

    }
    public void ToolDeselected()
    {
        if (placementState == State.MIDDLE)
        {
            placePoint(true);
            placePoint(true);
        }
        if (placementState == State.LAST)
        {
            placePoint(true);
        }
    }
}