using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//TODO: Doesn't work: angle check, disattaching the card when moving to the sides
public class Keycard : XRGrabInteractable
{
    [Header("Card Reader to be used")]
    [Space(2)]
    [SerializeField] CardReader cardReader;
    GameObject cardRailTrack;
    float railTrackX;
    float railTrackZ;
    bool isOnRailTrack = false;

    void Start()
    {
        this.gameObject.SetActive(false);
        cardRailTrack = cardReader.transform.Find("Card Rail Track").gameObject;
        railTrackX = cardRailTrack.transform.position.x;
        railTrackZ = cardRailTrack.transform.position.z;
    }

    void Update()
    {
        if (isOnRailTrack == true)
        {
            StickToTheReader();
        }
    }

    public void ActivateCard()
    {
        this.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == cardReader.gameObject)
        {
            isOnRailTrack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == cardReader.gameObject)
        {
            isOnRailTrack = false;
        }
    }

    void StickToTheReader()
    {
        this.transform.position = new Vector3(railTrackX, this.transform.position.y, railTrackZ);
    }
}
