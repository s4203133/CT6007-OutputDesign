using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorRemover {
    [SerializeField] private float centreRadius;
    private float centreRadiusSqrd;

    //[Header("OTHER CHUNKS")]
    //[SerializeField] private int frequency;
    //[SerializeField] private Vector2 radiusRange;
    //[SerializeField] private float positionRadius;
    //private FloorChunk[] chunksToRemove;

    public void Init() {
        centreRadiusSqrd = centreRadius * centreRadius;
        //chunksToRemove = new FloorChunk[frequency];
    }

    public void ClearSpaceAcrossFloor(ref List<Node> nodes) {
        RemoveCentreOfFloor(ref nodes);
        //RemoveRandomSectionsOfFloor(ref nodes);
    }

    private void RemoveCentreOfFloor(ref List<Node> nodes) {
        int count = nodes.Count;
        for (int i = 0; i < count; i++) {
            if (Vector3.SqrMagnitude(nodes[i].position) <= centreRadiusSqrd) {
                nodes.RemoveAt(i);
                i--;
                count--;
            }
        }
    }

    // private void RemoveRandomSectionsOfFloor(ref List<Node> nodes) {
    //     GenerateRandomChunks();
    //     int count = nodes.Count;
    //     for (int i = 0; i < frequency; i++) {
    //         for (int j = 0; j < count; j++) {
    //             if (Vector3.SqrMagnitude(chunksToRemove[i].position - nodes[j].position) <= chunksToRemove[i].radius) {
    //                 nodes.RemoveAt(j);
    //                 j--;
    //                 count--;
    //             }
    //         }
    //     }
    // }
    // 
    // private void GenerateRandomChunks() {
    //     for(int i = 0; i < frequency; i++) {
    //         chunksToRemove[i].position = new Vector2(
    //             Random.Range(centreRadius, positionRadius) * RandomModifier(),
    //             Random.Range(centreRadius, positionRadius) * RandomModifier());
    //         float radius = Random.Range(radiusRange.x, radiusRange.y);
    //         chunksToRemove[i].radius = radius * radius;
    //     }
    // }

    private float RandomModifier() {
        int choice = Random.Range(0, 2);
        if (choice == 0) { return 1; }
        else { return -1; }
    }

    public struct FloorChunk {
        public Vector2 position;
        public float radius;
    }
}