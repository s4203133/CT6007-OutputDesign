using System;
using UnityEngine;

public class Doorway : MonoBehaviour
{

    public static Action soldierWalkedThroughDoor;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 8) {
            soldierWalkedThroughDoor?.Invoke();
        }
    }
}
