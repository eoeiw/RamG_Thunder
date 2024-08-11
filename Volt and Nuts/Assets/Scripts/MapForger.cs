using System.Collections.Generic;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{
    public int numberOfRooms = 10; // Total number of rooms to generate
    public GameObject roomPrefab; // Prefab for visualizing rooms
    public float roomSize = 1.0f; // Size of each room on the minimap

    private List<Room> rooms = new List<Room>();

    void Start()
    {
        GenerateRooms();
        VisualizeRooms();
    }

    void GenerateRooms()
    {
        // Start with the initial room as the start room
        Room startRoom = new Room(Vector2Int.zero, 0); // Start room (type 0)
        rooms.Add(startRoom);

        // Generate remaining rooms
        for (int i = 1; i < numberOfRooms; i++)
        {
            Vector2Int newRoomPosition;
            do
            {
                // Select a random existing room
                Room existingRoom = rooms[Random.Range(0, rooms.Count)];

                // Determine a random adjacent position
                newRoomPosition = GetRandomAdjacentPosition(existingRoom.Position);

            } while (IsRoomPositionOccupied(newRoomPosition));

            // Add regular rooms with random types
            Room newRoom = new Room(newRoomPosition, Random.Range(3, 9));
            rooms.Add(newRoom);
        }

        // Assign special room types
        if (rooms.Count >= 3)
        {
            rooms[rooms.Count - 2].Info = 1; // Treasure room (type 1)
            rooms[rooms.Count - 1].Info = 2; // End room (type 2)
        }
    }

    Vector2Int GetRandomAdjacentPosition(Vector2Int currentPosition)
    {
        // Get a random direction: 0=right, 1=up, 2=left, 3=down
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0: return currentPosition + Vector2Int.right;
            case 1: return currentPosition + Vector2Int.up;
            case 2: return currentPosition + Vector2Int.left;
            case 3: return currentPosition + Vector2Int.down;
            default: return currentPosition; // This should never happen
        }
    }

    bool IsRoomPositionOccupied(Vector2Int position)
    {
        foreach (Room room in rooms)
        {
            if (room.Position == position)
            {
                return true;
            }
        }
        return false;
    }

    void VisualizeRooms()
    {
        foreach (Room room in rooms)
        {
            Vector3 worldPosition = new Vector3(room.Position.x * roomSize, room.Position.y * roomSize, 0);
            GameObject roomObject = Instantiate(roomPrefab, worldPosition, Quaternion.identity);

            // Change color based on room type
            Color roomColor = Color.white;
            switch (room.Info)
            {
                case 0: roomColor = Color.green; break; // Start room
                case 1: roomColor = Color.yellow; break; // Treasure room
                case 2: roomColor = Color.red; break; // End room
            }

            roomObject.GetComponent<Renderer>().material.color = roomColor;
        }
    }

    public class Room
    {
        public Vector2Int Position { get; private set; }
        public int Info { get; set; }

        public Room(Vector2Int position, int info)
        {
            Position = position;
            Info = info;
        }
    }
}



