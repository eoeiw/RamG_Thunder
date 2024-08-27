using UnityEngine;

public class Room
{
    public Vector2Int Position { get; private set; }
    public int Info { get; set; } // 방의 타입이나 기타 정보
    public GameObject roomObject; // 이 방을 나타내는 GameObject

    public Room(Vector2Int position, int info)
    {
        Position = position;
        Info = info;
    }
}


