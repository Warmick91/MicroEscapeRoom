using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Keycard : XRGrabInteractable
{
    [Header("Card Reader to be used")]
    [Space(2)]
    [SerializeField] CardReader cardReader;
    GameObject _cardRailTrack;
    float _railTrackX;
    float _railTrackZ;
    bool _isOnRailTrack = false;
    XRBaseInteractable _cardInteractable;
    GameObject _interactingHand;
    float maxHandDistance = 0.2f;

    void Start()
    {
        this.gameObject.SetActive(false);
        _cardInteractable = GetComponent<XRBaseInteractable>();
        _cardRailTrack = cardReader.transform.Find("Card Rail Track").gameObject;
        _railTrackX = _cardRailTrack.transform.position.x;
        _railTrackZ = _cardRailTrack.transform.position.z;
    }

    void Update()
    {
        if (!IsHandTooFar())
        {
            StickToTheReader();
        }
        //Debug.Log(_isOnRailTrack);
    }

    public void ActivateCard()
    {
        this.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == cardReader.gameObject)
        {
            _isOnRailTrack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == cardReader.gameObject)
        {
            _isOnRailTrack = false;
        }
    }

    bool IsHandTooFar()
    {
        if (_interactingHand == null)
        {
            return true;
        }

        Vector3 handPosition = _interactingHand.transform.position;
        Vector3 railTrackPosition = _cardRailTrack.transform.position;

        bool isHandTooFarOnZAxis = handPosition.z < railTrackPosition.z - maxHandDistance;
        bool isHandTooFarOnXAxis = handPosition.x > railTrackPosition.x + maxHandDistance
                                   || handPosition.x < railTrackPosition.x - maxHandDistance;

        return isHandTooFarOnZAxis || isHandTooFarOnXAxis;
    }

    void StickToTheReader()
    {
        if (_isOnRailTrack == true && _cardInteractable.interactorsSelecting.Count > 0 && _cardInteractable.interactorsSelecting[0] is XRDirectInteractor)
        {
            this.transform.rotation = Quaternion.Euler(-90f, 0, -90f);
            this.transform.position = new Vector3(_railTrackX, this.transform.position.y, _railTrackZ);
        }
    }

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
}
