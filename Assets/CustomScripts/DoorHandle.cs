using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : XRBaseInteractable
{
    private GameObject _interactingHand = null;
    private Vector3 _handForwardVector;
    [Header("Door To Move")]
    [SerializeField] Transform doorTransform;
    float minHoriztonalAngle = 0.3f;
    float minDistanceFromHandle = 0.2f;
    Vector3 startDoorPosition;
    Vector3 endDoorPosition;
    float doorSlideSpeed = 2f;
    float doorWeight = 0.5f;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _interactingHand = args.interactorObject.transform.parent.gameObject;
        _interactingHand.GetComponentInChildren<XRDirectInteractor>().hideControllerOnSelect = false;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _interactingHand.GetComponentInChildren<XRDirectInteractor>().hideControllerOnSelect = true;
        _interactingHand = null;

    }

    void Start()
    {
        startDoorPosition = doorTransform.position;
        endDoorPosition = new Vector3(startDoorPosition.x, startDoorPosition.y, startDoorPosition.z - 1f);
    }

    void Update()
    {
        if (_interactingHand != null)
        {
            Vector3 handPosition = _interactingHand.transform.position;
            _handForwardVector = _interactingHand.transform.forward;
            Vector3 handleToHandVector = handPosition - transform.position;
            float handHandleAngle = Vector3.Dot(_handForwardVector, Vector3.right);

            Debug.Log("handHandleAngle: " + handHandleAngle + "/ minHoriztonalAngle: " + minHoriztonalAngle);
            Debug.Log("handleToHandVector magnitude: " + handleToHandVector.magnitude + "/ minDistanceFromHandle: " + minDistanceFromHandle);
            if (handHandleAngle <= minHoriztonalAngle && handHandleAngle >= -minHoriztonalAngle && handleToHandVector.magnitude >= minDistanceFromHandle)
            {
                Debug.Log("Door should be moving");
            }

            // if (isAngleCorrect() && getHandDistanceFromHandle() >= minDistanceFromHandle)
            // {
            //     doorTransform.position = Vector3.MoveTowards(doorTransform.position, _interactingHand.transform.position, Time.deltaTime * doorSlideSpeed); //Add door weight later
            //     Debug.Log("The door speed is: " + Time.deltaTime * doorSlideSpeed * doorWeight);
            // }
        }
    }

    // bool isAngleCorrect()
    // {
    //     _handForwardVector = _interactingHand.transform.forward;
    //     float handHandleAngle = Vector3.Dot(_handForwardVector, transform.forward);

    //     if (handHandleAngle >= minHoriztonalAngle)
    //     {
    //         //Debug.Log("Hand Forward Vector:" + handHandleAngle);
    //         return true;
    //     }
    //     else
    //     {
    //         //Debug.Log("Wrong Hand Angle");
    //         return false;
    //     }
    // }

    // float getHandDistanceFromHandle()
    // {
    //     return Vector3.Distance(transform.position, _interactingHand.transform.position);
    // }
}
