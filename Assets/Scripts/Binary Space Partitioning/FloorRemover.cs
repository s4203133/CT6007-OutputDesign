using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorRemover {
    [SerializeField] private float centreRadius;
    private float centreRadiusSqrd;

    public void Initialise() {
        centreRadiusSqrd = centreRadius * centreRadius;
    }

    public void ClearSpaceAcrossFloor(ref List<Node> nodes) {
        RemoveCentreOfFloor(ref nodes);
    }

    private void RemoveCentreOfFloor(ref List<Node> nodes) {
        // Remove any rooms within disatnce of the centre
        int count = nodes.Count;
        for (int i = 0; i < count; i++) {
            if (Vector3.SqrMagnitude(nodes[i].position) <= centreRadiusSqrd) {
                nodes.RemoveAt(i);
                i--;
                count--;
            }
        }
    }
}