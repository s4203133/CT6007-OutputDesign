using UnityEngine;

public class RoomDivider
{

    private static NewRoomPair CreateRooms(Node currentRoom, Vector2 room1Position, Vector2 room1Size, Vector2 room2Position, Vector2 room2Size) {
        Node room1 = new Node(currentRoom, room1Position, (int)room1Size.x, (int)room1Size.y);
        Node room2 = new Node(currentRoom, room2Position, (int)room2Size.x, (int)room2Size.y);
        return new NewRoomPair(room1, room2);
    }

    public static NewRoomPair CreateHorizontalRooms(Node currentRoom, CastleData roomCreationData) {
        int newRoom1Size = Random.Range(roomCreationData.minRoomWidth, currentRoom.width - roomCreationData.minRoomWidth);
        int newRoom2Size = currentRoom.width - newRoom1Size;

        Vector2 room1Position = new Vector2(currentRoom.position.x - (currentRoom.width * 0.5f) + (newRoom1Size * 0.5f), currentRoom.position.y);
        Vector2 room2Position = new Vector2(currentRoom.position.x + (currentRoom.width * 0.5f) - (newRoom2Size * 0.5f), currentRoom.position.y);

        return CreateRooms(currentRoom, room1Position, new Vector2(newRoom1Size, currentRoom.length), room2Position, new Vector2(newRoom2Size, currentRoom.length));
    }

    public static NewRoomPair CreateVerticalRooms(Node currentRoom, CastleData roomCreationData) {
        int newRoom1Size = Random.Range(roomCreationData.minRoomLength, currentRoom.length - roomCreationData.minRoomLength);
        int newRoom2Size = currentRoom.length - newRoom1Size;

        Vector2 room1Position = new Vector2(currentRoom.position.x, currentRoom.position.y - (currentRoom.length * 0.5f) + (newRoom1Size * 0.5f));
        Vector2 room2Position = new Vector2(currentRoom.position.x, currentRoom.position.y + (currentRoom.length * 0.5f) - (newRoom2Size * 0.5f));

        return CreateRooms(currentRoom, room1Position, new Vector2(currentRoom.width, newRoom1Size), room2Position, new Vector2(currentRoom.width, newRoom2Size));
    }

    public static NewRoomPair CreateRandomRooms(Node currentRoom, CastleData roomCreationData) {
        int i = Random.Range(0, 2);
        if(i == 0) {
            return CreateHorizontalRooms(currentRoom, roomCreationData);
        } else {
            return CreateVerticalRooms(currentRoom, roomCreationData);
        }
    }
}

public struct NewRoomPair {
    public Node room1;
    public Node room2;

    public NewRoomPair(Node room1, Node room2) {
        this.room1 = room1;
        this.room2 = room2;
    }
}
