using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Set the interaction layer of an interactor
/// </summary>
public class SetInteractionLayer : MonoBehaviour
{
    [Tooltip("The layer that's switched to")]
    public LayerMask targetLayer = 0;

    private XRBaseInteractor interactor = null;
    private LayerMask originalLayer = 0;

    private void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
        originalLayer = interactor.interactionLayerMask;
    }

    public void SetTargetLayer()
    {
        interactor.interactionLayerMask = targetLayer;
    }

    public void SetOriginalLayer()
    {
        interactor.interactionLayerMask = originalLayer;
    }

    public void ToggleTargetLayer(bool value)
    {
        if (value)
        {
            SetTargetLayer();
        }
        else
        {
            SetOriginalLayer();
        }
    }

}
