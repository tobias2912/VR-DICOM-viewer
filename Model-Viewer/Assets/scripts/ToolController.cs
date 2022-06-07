using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolController : MonoBehaviour
{
    private Tooltype currentToolType;
    private Tool currentTool;
    public GameObject toolMenu;
    public GameObject MeasureTool;
    public GameObject AngleTool;
    [Header("Screw type if relevant")]
    public GameObject ImplantTool;
    private MeasurePlacement measureController;
    private AngleMeasurePlacement angleController;
    private PlaceImplant implantController;
    private XRInteractorLineVisual measureRayLeft;
    private XRInteractorLineVisual measureRayRight;
    [SerializeField]
    private GameObject measureRayL;
    [SerializeField]
    private GameObject measureRayR;
    [SerializeField]
    private GameObject rightRay;
    [SerializeField]
    private GameObject leftRay;
    private bool isHoveringUi;
    private bool selectEntered;
    private List<Tool> tools;

    public void Start()
    {
        measureController = MeasureTool.GetComponent<MeasurePlacement>();
        angleController = AngleTool.GetComponent<AngleMeasurePlacement>();
        implantController = ImplantTool.GetComponent<PlaceImplant>();
        measureRayLeft = measureRayL.GetComponent<XRInteractorLineVisual>();
        measureRayRight = measureRayR.GetComponent<XRInteractorLineVisual>();
        tools = new List<Tool>();
        tools.Add(measureController);
        tools.Add(angleController);
        tools.Add(implantController);
    }

    public void setCurrentTool(Tooltype type)
    {

        currentTool?.ToolDeselected();
        Tool tool = getTool(type);
        tool.ToolSelected(type);

        setLinesVisible(false);
        if (type == Tooltype.MeasureDistance || type == Tooltype.MeasureAngle)
        {
            setLinesVisible(true);
        }
        currentToolType = type;
        currentTool = tool;
        print("tool set to " + currentToolType);
    }

    public Tooltype getCurrentTool()
    {
        return currentToolType;
    }
    private void setLinesVisible(bool value)
    {
        measureRayLeft.enabled = value;
        measureRayRight.enabled = value;
    }

    private bool isAimingAtButton(bool isrightcontroller)
    {
        XRRayInteractor ray;
        if (isrightcontroller)
        {
            ray = rightRay.GetComponent<XRRayInteractor>();
        }
        else
        {
            ray = leftRay.GetComponent<XRRayInteractor>();
        }
        bool hit = ray.TryGetCurrentUIRaycastResult(out RaycastResult res);
        if (!hit) return false;
        if (res.gameObject.GetComponentsInParent<Button>().Length > 0)
        {
            return true;
        }
        return false;
    }
    private Tool getTool(Tooltype tooltype)
    {
        if (tooltype == Tooltype.MeasureDistance)
        {
            return measureController;
        }
        if (tooltype == Tooltype.MeasureAngle)
        {
            return angleController;
        }
        if (tooltype.isImplant())
        {
            return implantController;
        }
        throw new System.Exception("no Tool for " + tooltype);
    }

    public void activateCurrentTool(bool isRightController)
    {
        if (isAimingAtButton(isRightController))
        {
            return;
        }
        if (toolMenu.activeSelf || currentToolType == Tooltype.None)
        {
            return;
        }
        print("activate current tool " + currentToolType);
        currentTool.ToolActivated(isRightController);
    }

}
public enum Tooltype
{
    None,
    MeasureDistance,
    MeasureAngle,
    Screw50mm,
    Screw80mm,
    Plate,
}
public static class ToolExtension
{
    public static bool isImplant(this Tooltype type)
    {
        return type == Tooltype.Screw50mm || type == Tooltype.Screw80mm || type == Tooltype.Plate;
    }
}
