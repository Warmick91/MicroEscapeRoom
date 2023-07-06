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

    [Header("Keypad Lights")]
    [SerializeField] private GameObject _redLight;
    [SerializeField] private GameObject _greenLight;
    [SerializeField] private Material _emissiveRedMaterial;
    [SerializeField] private Material _emissiveGreenMaterial;
    [SerializeField] private Light _pointLightRed;
    [SerializeField] private Light _pointLightGreen;

    [Header("Door Bar To Unlock")]
    [SerializeField]
    private DoorPadlock _doorToUnlock;

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
            if (cardForwardness < minAcceptableForwardness)
            {
                isSwipeValid = false;
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
            StartCoroutine(LightUp("red"));
            return;
        }


        exitPosition = _keycardTransform.position;
        float currentSwipeLength = Vector3.Distance(entryPosition, exitPosition);
        if (isSwipeValid && currentSwipeLength >= correctSwipeLength)
        {
            Debug.Log("Swipe Success");
            isSwipeValid = false;
            StartCoroutine(LightUp("green"));
            _doorToUnlock.DisablePadlock();
        }
        else
        {
            Debug.Log("Swipe Failed");
            isSwipeValid = false;
            StartCoroutine(LightUp("red"));
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return false;
    }

    public IEnumerator LightUp(string color)
    {
        if (color == "red")
        {
            Material originalMaterial = _redLight.GetComponent<Renderer>().material;
            _redLight.GetComponent<Renderer>().material = _emissiveRedMaterial;
            _pointLightRed.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _redLight.GetComponent<Renderer>().material = originalMaterial;
            _pointLightRed.enabled = false;
        }
        else if (color == "green")
        {
            Material originalMaterial = _greenLight.GetComponent<Renderer>().material;
            _greenLight.GetComponent<Renderer>().material = _emissiveGreenMaterial;
            _pointLightGreen.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _greenLight.GetComponent<Renderer>().material = originalMaterial;
            _pointLightGreen.enabled = false;
        }
    }
}
