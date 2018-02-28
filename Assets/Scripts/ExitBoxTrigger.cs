//------------------------------------------------------------------------
//
// (C) Copyright 2017 Urahimono Project Inc.
//
//------------------------------------------------------------------------
using UnityEngine;

public class ExitBoxTrigger : MonoBehaviour
{
    public event System.Action onExit  = () => {};


    private void OnTriggerExit(Collider i_other)
    {
        onExit();
    }

    public void Initialize(Transform i_parent, BoxCollider i_sourceCollider, int i_triggerLayer)
    {
        transform.SetParent(i_parent, false);


        var collider = gameObject.AddComponent<BoxCollider>();

        collider.isTrigger = true;
        collider.center = i_sourceCollider.center;
        collider.size = i_sourceCollider.size;

        gameObject.layer = i_triggerLayer;
    }

}
 // class ExitTrigger
