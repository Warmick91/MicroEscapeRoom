using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPadlock : MonoBehaviour
{   
    [SerializeField] GameObject theActualPadlock;
    public static bool isPadlockActive = true;

    public void DisablePadlock()
    {
        theActualPadlock.SetActive(false);
        isPadlockActive = false;
    }

}
