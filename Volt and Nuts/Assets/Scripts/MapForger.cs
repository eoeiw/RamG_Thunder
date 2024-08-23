using System.Collections.Generic;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{
    public int numberOfRooms = 10;
    public GameObject roomPrefab;
    public float roomSize = 1.0f;

    public List<Room> rooms = new List<Room>();

    public static RandomMapGenerator instance = null; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    void Start()
    {
        GenerateRooms();
        PrintRooms();
        MoveToScene1();
    }

    void GenerateRooms()
    {
        Room startRoom = new Room(Vector2Int.zero, 0);
        rooms.Add(startRoom);

        for (int i = 1; i < numberOfRooms; i++)
        {
            Vector2Int newRoomPosition;
            do
            {
                Room existingRoom = rooms[Random.Range(0, rooms.Count)];
                newRoomPosition = GetRandomAdjacentPosition(existingRoom.Position);

            } while (IsRoomPositionOccupied(newRoomPosition));

            Room newRoom = new Room(newRoomPosition, Random.Range(3, 9));
            rooms.Add(newRoom);
        }

        if (rooms.Count >= 3)
        {
            rooms[rooms.Count - 2].Info = 1; // Treasure room
            rooms[rooms.Count - 1].Info = 2; // End room
        }
    }

    Vector2Int GetRandomAdjacentPosition(Vector2Int currentPosition)
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0: return currentPosition + Vector2Int.right;
            case 1: return currentPosition + Vector2Int.up;
            case 2: return currentPosition + Vector2Int.left;
            case 3: return currentPosition + Vector2Int.down;
            default: return currentPosition;
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
    
    void PrintRooms()
    {
        foreach (Room room in rooms)
        {
            Debug.Log($"방 위치: {room.Position}, 방 타입: {room.Info}");
        }
    }

    void VisualizeRooms()
    {
        foreach (Room room in rooms)
        {
            Vector3 worldPosition = new Vector3(room.Position.x * roomSize, room.Position.y * roomSize, 0);
            GameObject roomObject = Instantiate(roomPrefab, worldPosition, Quaternion.identity);

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

    void MoveToScene1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene1");
    }
}



