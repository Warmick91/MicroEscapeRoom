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
    float minDistanceFromHandle = 0.05f;
    Vector3 startDoorPosition;
    Vector3 endDoorPosition;
    float doorSlideSpeed = 2f;
    //float doorWeight = 0.5f;

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
        if (_interactingHand != null && !DoorPadlock.isPadlockActive)
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
                float lerpDoorPositionZ = Mathf.Lerp(doorTransform.position.z, handPosition.z, Time.deltaTime * doorSlideSpeed);
                Vector3 newDoorPosition = new Vector3(startDoorPosition.x, startDoorPosition.y, lerpDoorPositionZ);
                doorTransform.position = newDoorPosition;

                CheckDoorBounds();
            }
        }
    }

    void CheckDoorBounds()
    {
        if (doorTransform.position.z <= endDoorPosition.z)
        {
            doorTransform.position = endDoorPosition;
        }
        else if (doorTransform.position.z >= startDoorPosition.z)
        {
            doorTransform.position = startDoorPosition;
        }
    }
}
