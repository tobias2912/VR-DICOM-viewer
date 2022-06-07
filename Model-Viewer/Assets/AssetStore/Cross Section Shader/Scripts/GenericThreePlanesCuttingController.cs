using UnityEngine;
using System.Collections;

public class GenericThreePlanesCuttingController : MonoBehaviour
{

    public GameObject planeIntersectionObj;
    private GameObject plane1;
    private GameObject plane2;
    private GameObject plane3;
    Material mat;
    private Vector3 normal1;
    private Vector3 position1;
    private Vector3 normal2;
    private Vector3 position2;
    private Vector3 normal3;
    private Vector3 position3;
    private Renderer rend;
    public new bool cutoffEnabled = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if(rend == null){
            throw new System.Exception();
        }
    }
    void Update()
    {
        if(!cutoffEnabled){
            return;
        }
        if(planeIntersectionObj == null){
            throw new System.Exception("cutoff position missing");
        }
        plane1 = planeIntersectionObj;
        plane2 = planeIntersectionObj;
        plane3 = planeIntersectionObj;
        UpdateShaderProperties();
    }

    private void UpdateShaderProperties()
    {
        if(plane1 == null || plane2 == null){
            throw new System.Exception();
        }
        normal1 = plane1.transform.TransformVector(new Vector3(0, 0, -1));
        normal2 = plane2.transform.TransformVector(new Vector3(0, 0, -1));
        normal3 = plane3.transform.TransformVector(new Vector3(0, 0, -1));
        position1 = plane1.transform.position;
        position2 = plane2.transform.position;
        position3 = plane3.transform.position;
        rend.material.SetVector("_Plane1Normal", normal1);
        rend.material.SetVector("_Plane1Position", position1);
        rend.material.SetVector("_Plane2Normal", normal2);
        rend.material.SetVector("_Plane2Position", position2);
        rend.material.SetVector("_Plane3Normal", normal3);
        rend.material.SetVector("_Plane3Position", position3);
    }
}
