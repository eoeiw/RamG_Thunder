using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리용 네임스페이스 추가

public class RandomMapGenerator : MonoBehaviour
{
    public int numberOfRooms = 10;
    public GameObject roomPrefab;
    public float roomSize = 1.0f;

    public List<Room> rooms = new List<Room>();

    public static RandomMapGenerator instance = null; 
    public int stageNumber = 0; // 스테이지 넘버 추가

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 각 씬이 로드될 때 새로운 스테이지를 생성
        if (scene.name == "Map_Inventor")
        {
            GenerateNewStage(); // 새로운 맵 생성
        }
    }

    public void CompleteStage()
    {
        // 스테이지를 클리어하면 Map_Inventor로 돌아감
        SceneManager.LoadScene("Map_Inventor");
    }

    void GenerateNewStage()
    {
        // 방 리스트 초기화 및 새 맵 생성
        rooms.Clear();
        GenerateRooms();
        MoveToCurrentScene();
    }

    void MoveToCurrentScene()
    {
        // 무조건 Scene1로 이동
        SceneManager.LoadScene("Scene1");
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
            rooms[rooms.Count - 1].Info = 2; // End room (출구)
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
}
