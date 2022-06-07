using System.Collections.Generic;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

namespace UnityEngine.XR.Interaction.Toolkit
{
    /*
    extension of interactor to only allowing canSelect() to trigger when no other item is grabbed
    also a single item is hovered, and that item is always selected first
    */
    public class XRDirectInteractorSingleHover : XRDirectInteractor
    {
        public GameObject grabmodeobj;
        public override void GetValidTargets(List<XRBaseInteractable> validTargets)
        {
            base.GetValidTargets(validTargets);
            return;
            int validCount = validTargets.Count;
            float minDistance = float.MaxValue;
            XRBaseInteractable minBaseInteractable = null;

            // Calculate distance squared to interactor's attach transform and add to validTargets (which is sorted before returning)
            foreach (var interactable in validTargets)
            {
                screwColorChanger component = null;
                if (interactable.gameObject.TryGetComponent<screwColorChanger>(out component))
                {
                    //screw has high priority and ignores distance
                    minBaseInteractable = interactable;
                    continue;
                }
                float dist = interactable.GetDistanceSqrToInteractor(this);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    minBaseInteractable = interactable;
                }
            }
            if (minBaseInteractable != null)
            {
                validTargets = new List<XRBaseInteractable>();
                validTargets.Add(minBaseInteractable);
            }
            // print("valid:" + validTargets.Count + " / "+validCount+" name: " + (validTargets.Count > 0 ? validTargets[0].ToString() : "-"));
        }

        public override bool CanHover(XRBaseInteractable interactable)
        {
            if (validTargets.Count == 0)
            {
                return false;
            }
            return validTargets[0] == interactable && base.CanHover(interactable);
        }

        private XRBaseInteractable closestInteractable(List<XRBaseInteractable> validTargets)
        {
            if (validTargets.Count == 0)
            {
                throw new System.Exception("empty list");
            }
            float bestDistance = 999999f;
            XRBaseInteractable closestInteractable = null;
            foreach (XRBaseInteractable validTarget in validTargets)
            {
                Collider col = validTarget.gameObject.GetComponent<Collider>();
                Vector3 closestEdge = col.ClosestPoint(this.transform.position);
                float dist = Vector3.Distance(closestEdge, this.transform.position);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    closestInteractable = validTarget;
                }
            }
            print("closest interactable was " + closestInteractable);
            return closestInteractable;

        }
        public override bool CanSelect(XRBaseInteractable interactable)
        {
            GrabModeController grabModeController = grabmodeobj.GetComponent<GrabModeController>();
            bool isAlreadySelected = grabModeController.isCurrentlyGrabbing(interactable);
            if (isAlreadySelected)
            {
                return true;
            }
            if(grabModeController.isCurrentlyGrabbing()){
                return false;
            }

            if (validTargets[0] == interactable)
            {
                if (base.CanSelect(interactable))
                {

                    return true;
                }
            }
            return false;
            // return (validTargets.Count == 0 ||
            //         validTargets[0] == interactable &&
            //         base.CanSelect(interactable));
        }
    }
}