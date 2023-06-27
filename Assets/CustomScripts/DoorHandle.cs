using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : XRBaseInteractable
{
    private GameObject _interactingHand = null;
    private Vector3 _handForwardVector;
    float minHoriztonalAngle = -0.7f;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _interactingHand = args.interactorObject.transform.parent.gameObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _interactingHand = null;
    }

    void Update()
    {
        if (_interactingHand != null)
        {
            _handForwardVector = _interactingHand.transform.forward;
            float handHandleAngle = Vector3.Dot(_handForwardVector, transform.forward);
            if (handHandleAngle >= minHoriztonalAngle)
            {
                Debug.Log("Hand Forward Vector:" + handHandleAngle);
            }
            else
            {
                Debug.Log("Wrong Hand Angle");
            }
        }
    }
}
