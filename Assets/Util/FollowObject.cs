using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public enum UpdateType
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

    public enum MovementType
    {
        Lerp,
        MoveTowards,
        Instant
    }

    [SerializeField] private Transform _objectToFollow;
    [SerializeField] private Vector3 _margin = Vector3.zero;
    [SerializeField] private Vector3 _orbitRotation = Vector3.zero;

    [Header("Constraints")]
    [SerializeField] private bool _x = false;
    [SerializeField] private bool _y = false;
    [SerializeField] private bool _z = false;

    [Header("Movement information")]

    [SerializeField] private UpdateType _updateType = UpdateType.FixedUpdate;
    [SerializeField] private MovementType _movementType = MovementType.Lerp;
    [SerializeField] [Range(0.01f, 1f)] private float _movementMultiplier = 0.1f;


    // === LOCAL VARIABLES ===

    private Vector3 _distToObject;
    private Quaternion _startingRot;
    private bool _pause = false;


    void Start()
    {
        // the original distance from the object + margin will be the distance to the _objectToFollow
        if (_objectToFollow != null)
        {
            _distToObject = transform.position - _objectToFollow.position;
            _startingRot = transform.rotation;
        }

    }

    // Call the Move() function in the requested function
    void Update() { if(_updateType == UpdateType.Update && _pause == false) Move(); }
    void FixedUpdate() { if(_updateType == UpdateType.FixedUpdate && _pause == false) Move(); }
    void LateUpdate() { if (_updateType == UpdateType.LateUpdate && _pause == false) Move(); }

    /// <summary>
    /// Set the target to follow and initialise accordingly
    /// </summary>
    /// <param name="newTarget"> New target to follow </param>
    public void SetTarget(Transform newTarget)
    {
        _objectToFollow = newTarget;
        _distToObject = transform.position - _objectToFollow.position;
        _startingRot = transform.rotation;
    }

    /// <summary>
    /// Set the margin to follow the target with
    /// </summary>
    /// <param name="newTarget"> New margin </param>
    public void SetMargin(Vector3 newMargin)
    {
        _margin = newMargin;
    }

    public Vector3 GetTargetPosition()
    {
        // if the _objectToFollow is not set we do not run the code to follow it
        if (_objectToFollow == null) return Vector3.zero;

        Vector3 current = transform.position;
        Vector3 follow = _objectToFollow.position;

        // calculate position target with constraints and margin in mind
        Vector3 _target = new Vector3
        (
            ((_x) ? current.x : follow.x + _distToObject.x + _margin.x),
            ((_y) ? current.y : follow.y + _distToObject.y + _margin.y),
            ((_z) ? current.z : follow.z + _distToObject.z + _margin.z)
        );

        //transform.rotation = _startingRot * Quaternion.Euler(_orbitRotation);

        return _target;
    }

    // Move the current transform to the calculated position to follow _objectToFollow
    private void Move() 
    {
        // if the _objectToFollow is not set we do not run the code to follow it
        if (_objectToFollow == null) return;

        Vector3 current = transform.position;

        // calculate position target with constraints and margin in mind
        Vector3 _target = GetTargetPosition();

        // set the position with the requested function
        switch (_movementType)
        {
            case MovementType.Lerp:
                transform.position = Vector3.Lerp(current, _target, _movementMultiplier);
                break;
            case MovementType.MoveTowards:
                transform.position = Vector3.MoveTowards(current, _target, _movementMultiplier);
                break;
            case MovementType.Instant:
                transform.position = _target;
                break;
        }
    }
}
