using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BinarySpacePartitioning
{
    private CastleData castleData;
    [SerializeField] private int maxIterations;

    private List<Node> castleRooms;
    [SerializeField] private GameObject roomPrefab;

    [SerializeField] private FloorRemover floorRemover;

    Node rootNode;

    public void Initialise(CastleData data) {
        castleData = data;
        castleRooms = new List<Node>();
        floorRemover.Init();
    }

    public void CreateFloorMap() {
        rootNode = new Node(null, Vector2.zero, castleData.width, castleData.length);
        BuildRooms();
    }

    private void BuildRooms() {
        Queue<Node> rooms = new Queue<Node>();
        rooms.Enqueue(rootNode);

        int iteration = 0;
        while(iteration < maxIterations || rooms.Count > 0) {
            Node currentRoom = rooms.Dequeue();
            if (RoomIsLargeEnoughToDivide(currentRoom)) { 
                NewRoomPair newRooms = DivideRoom(currentRoom);
                rooms.Enqueue(newRooms.room1);
                rooms.Enqueue(newRooms.room2);
                castleRooms.Add(newRooms.room1);
                castleRooms.Add(newRooms.room2);
            }
            iteration++;
            if (iteration >= maxIterations || rooms.Count == 0) {
                break;
            }
        }
        GetFinalRooms();
        floorRemover.ClearSpaceAcrossFloor(ref castleRooms);
        CreateRooms();
    }

    private void GetFinalRooms() {
        int count = castleRooms.Count;
        for(int i = 0; i < count; i++) {
            if (castleRooms[i].children.Count > 0) {
                castleRooms.RemoveAt(i);
                i--;
                count--;
            }
        }
    }

    private void CreateRooms() {
        int count = castleRooms.Count;
        for(int i = 0; i < count; i++) { 
            InstantiateRoom("Room " + i, castleRooms[i]);
        }
    }

    void InstantiateRoom(string name, Node room) {
        GameObject roomVisuals = GameObject.Instantiate(roomPrefab, new Vector3(room.position.x, 0, room.position.y), Quaternion.identity);
        roomVisuals.transform.localScale = new Vector3(room.width, 1, room.length);
        roomVisuals.name = name;
    }

    private NewRoomPair DivideRoom(Node room) {
        bool canCreateHorizontalRoom = room.width > castleData.minRoomWidth;
        bool canCreateVerticalRoom = room.length > castleData.minRoomLength;

        if (canCreateHorizontalRoom && canCreateVerticalRoom) {
            return RoomDivider.CreateRandomRooms(room, castleData);
        } else if (canCreateHorizontalRoom && !canCreateVerticalRoom) {
            return RoomDivider.CreateHorizontalRooms(room, castleData);
        } else if(canCreateVerticalRoom && !canCreateHorizontalRoom) {
            return RoomDivider.CreateVerticalRooms(room, castleData);
        }
        else {
            Debug.Log("Room could not be divided - ERROR!");
            return new NewRoomPair();
        }
    }

    private bool RoomIsLargeEnoughToDivide(Node node) {
        return (node.width >= castleData.minRoomWidth * 2 || node.length >= castleData.minRoomLength * 2);
    }

    public void ClearMap() {
        Room[] rooms = GameObject.FindObjectsOfType<Room>();
        for (int i = 0; i < rooms.Length; i++) {
            GameObject.Destroy(rooms[i].gameObject);
        }
        castleRooms.Clear();
    }
}
