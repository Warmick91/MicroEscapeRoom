using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    [Header("Keycard Interactor")]
    [SerializeField]
    private GameObject _keycard;
    private Transform _keycardTransform;
    private bool isSwipeValid = false;
    Vector3 entryPosition;
    Vector3 exitPosition;
    float entryTime;
    float minSwipeTime = 0.25f;
    float maxSwipeTime = 2f;
    float correctSwipeLength = 0.25f;
    float minAcceptableForwardness = 0.7f;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (isSwipeValid)
        {
            Vector3 keycardUp = _keycardTransform.forward;
            float cardForwardness = Vector3.Dot(keycardUp, Vector3.up);
            //Debug.Log("cardForwardness: " + cardForwardness);
            if (cardForwardness < minAcceptableForwardness)
            {
                isSwipeValid = false;
                Debug.Log("Wrong Card Angle");
            }
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        entryTime = Time.time;
        _keycardTransform = _keycard.transform;
        if (_keycardTransform != null)
        {
            isSwipeValid = true;
        }
        entryPosition = _keycardTransform.position;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        float exitTime = Time.time;
        if (exitTime - entryTime >= minSwipeTime && exitTime - entryTime <= maxSwipeTime)
        {
            isSwipeValid = true;
        }
        else
        {
            isSwipeValid = false;
            Debug.Log("Swipe Time incorrect: " + (exitTime - entryTime) + "/ Minimum Swipe Time: " + minSwipeTime + "/ Maximum Swipe Time: " + maxSwipeTime);
            return;
        }


        exitPosition = _keycardTransform.position;
        float currentSwipeLength = Vector3.Distance(entryPosition, exitPosition);
        if (isSwipeValid && currentSwipeLength >= correctSwipeLength)
        {
            Debug.Log("Swipe Success");
            isSwipeValid = false;
            Debug.Log("Swipe Length: " + currentSwipeLength + "/ Minimum Swipe Length: " + correctSwipeLength);
            Debug.Log("Time of Swipe: " + (exitTime - entryTime));
        }
        else
        {
            Debug.Log("Swipe Failed");
            isSwipeValid = false;
            Debug.Log("Swipe Length: " + currentSwipeLength + "/ Minimum Swipe Length: " + correctSwipeLength);
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return false;
    }
}
