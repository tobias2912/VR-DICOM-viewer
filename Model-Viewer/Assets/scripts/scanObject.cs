using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
represents a single scan piece/a fracture part
*/
public class scanObject : MonoBehaviour
{
    private Vector3 startpos;
    private Quaternion startRot;
    private Rigidbody m_Rigidbody;
    private bool returningPosition;
    private bool returningRotation;
    private Color originalColor;

    public float speed;
    public float rotationSpeed;
    public bool instantReset;
    public bool highlightOnHover;
    [Range(0.1f, 0.9f)]
    public float highlightIntensity = 0.5f;
    public Color highlightColor;
    
    void Start()
    {
        startpos = transform.localPosition;
        startRot = transform.localRotation;
        removeCollision();
    }
    public void setColor(Color newColor){
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = newColor;
        originalColor = newColor;
    }

    public void removeCollision()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //Ignore the collisions between scan objects layer
        Physics.IgnoreLayerCollision(3, 3);
    }

    public void resetPosition()
    {
        if (instantReset)
        {
            transform.localRotation = startRot;
            transform.localPosition = startpos;
            GetComponent<NetworkSyncTransform>().syncToServer(transform);
        }
        else
        {

            if (returningPosition || returningRotation)
            {
                print("already returning");
                return;
            }
            print("return to starting pos");
            returningRotation = true;
            returningPosition = true;
            Vector3 dist = startpos - transform.localPosition;
            float magnitude = dist.magnitude;
            speed *= magnitude;
        }
    }
    public void changeColorHoverEnter()
    {
        if (highlightOnHover)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material.color = Color.Lerp(originalColor, highlightColor, highlightIntensity);
        }
    }
    public void changeColorHoverExit()
    {
        if (highlightOnHover)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material.color = originalColor;
        }
    }
}
