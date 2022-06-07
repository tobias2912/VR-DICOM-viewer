using System;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/*
extension of grabbable class
allows to grab a object without snapping to a attach transform
some utility for other classes like triggering when grabbed
*/
public class offsetGrab : XRGrabInteractable
{
    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotrotation = Quaternion.identity;

    public bool currentlyGrabbed = false;
    public GameObject modeController;
    [Tooltip("if grabbing should trigger Separation Mode. Should be True for model parts")]
    public bool triggerSeparationMode = true;

    protected override void Awake()
    {
        base.Awake();
    }

    [System.Obsolete]
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        GrabModeController controller = null;
        controller = modeController.GetComponent<GrabModeController>();
        if(controller.isCurrentlyGrabbing()){
            return;
        }
        print("entered interact on "+gameObject.name);
        if (interactor == null)
        {
            throw new Exception("interactor not found");
        }
        if (modeController == null)
        {
            throw new Exception("modecontroller not found");
        }
        base.OnSelectEntering(interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
        if (controller == null)
        {
            throw new Exception("grabmodecontroller not found");
        }
        if (triggerSeparationMode)
        {
            if (controller.isSeparationMode())
            {
                //already triggered, do nothing
            }
            else
            {
                controller.CMDenableSeparationMode();
            }
        }
        currentlyGrabbed = true;
        print("interaction locked to "+gameObject.name);
    }
    [System.Obsolete]
    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        ResetAttachmentPoint(interactor);
        ClearInteractor(interactor);
        currentlyGrabbed = false;

    }
    private void StoreInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotrotation = interactor.attachTransform.localRotation;

    }
    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;

    }


    private void ResetAttachmentPoint(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = interactorPosition;
        interactor.attachTransform.localRotation = interactorRotrotation;
    }
    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = Vector3.zero;
        interactorRotrotation = Quaternion.identity;
    }
}
