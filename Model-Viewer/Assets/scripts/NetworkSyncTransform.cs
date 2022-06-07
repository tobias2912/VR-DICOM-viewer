using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Mirror;

[RequireComponent(typeof(offsetGrab))]
public class NetworkSyncTransform : NetworkBehaviour
{

    /*
    automatically syncs movement to server 
    whenever someone is holding a grabbable
    */
    [SerializeField]
    private float _posLerpRate = 15;
    [SerializeField]
    private float _rotLerpRate = 15;
    [SerializeField]
    private float _posThreshold = 0.1f;
    [SerializeField]
    private float _rotThreshold = 1f;

    [SyncVar]
    private Vector3 _lastPosition;

    [SyncVar]
    private Vector3 _lastRotation;

    public void syncTransformToServer(){
        CmdMove(transform.position, transform.rotation.eulerAngles);
    }

    public void syncToServer(Transform t){
        CmdMove(t.position, t.rotation.eulerAngles);
    }
    
    [Command(requiresAuthority = false)]
    private void CmdMove(Vector3 pos, Vector3 rot)
    {
        _lastPosition = pos;
        _lastRotation = rot;
    }

    void Update()
    {
        if (isLocalPlayer)
            return;
        if(GetComponent<offsetGrab>().currentlyGrabbed){
            // CmdMove(transform.position, transform.rotation.eulerAngles);
            syncToServer(transform);
        }
        InterpolatePosition();
        InterpolateRotation();
    }

    void Start()
    {
        if (isServer)
        {
            _lastPosition = transform.position;
            _lastRotation = transform.rotation.eulerAngles;
        }
    }
    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        var posChanged = IsPositionChanged();

        if (posChanged)
        {
            CmdSendPosition(transform.position);
            _lastPosition = transform.position;
        }

        var rotChanged = IsRotationChanged();

        if (rotChanged)
        {
            CmdSendRotation(transform.localEulerAngles);
            _lastRotation = transform.localEulerAngles;
        }

    }

    public void InterpolatePosition()
    {

        transform.position = Vector3.Lerp(transform.position, _lastPosition, Time.deltaTime * _posLerpRate);

    }

    private void InterpolateRotation()
    {

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_lastRotation), Time.deltaTime * _rotLerpRate);

    }


    [Command]
    public void CmdSendPosition(Vector3 pos)
    {

        _lastPosition = pos;

    }

    [Command]
    private void CmdSendRotation(Vector3 rot)
    {

        _lastRotation = rot;

    }

    private bool IsPositionChanged()
    {

        return Vector3.Distance(transform.position, _lastPosition) > _posThreshold;

    }

    private bool IsRotationChanged()
    {

        return Vector3.Distance(transform.localEulerAngles, _lastRotation) > _rotThreshold;

    }
}