using System;
using UnityEngine;

public class Doorway : MonoBehaviour
{

    public static Action soldierWalkedThroughDoor;

    private void OnTriggerEnter(Collider other) {
        // If a soldier walks through this doorway, broadcast an event (informs ML agent to recieve a penalty)
        if(other.gameObject.layer == 8) {
            soldierWalkedThroughDoor?.Invoke();
        }
    }
}
