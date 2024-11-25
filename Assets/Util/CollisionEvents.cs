using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.CoroutineExtensions;

public class CollisionEvents : MonoBehaviour
{
    public Action<Collision> CollisionEnterEvent;
    public Action<Collision> CollisionExitEvent;
    public Action<Collider> TriggerEnterEvent;
    public Action<Collider> TriggerExitEvent;

    void OnCollisionEnter(Collision col)
    {
        CollisionEnterEvent?.Invoke(col);
    }

    void OnCollisionExit(Collision col)
    {
        CollisionExitEvent?.Invoke(col);
    }

    void OnTriggerEnter(Collider col)
    {
        TriggerEnterEvent?.Invoke(col);
    }

    void OnTriggerExit(Collider col)
    {
        TriggerExitEvent?.Invoke(col);

        this.WaitSecondsToExecute(2, () =>
        {

        });
    }
}
