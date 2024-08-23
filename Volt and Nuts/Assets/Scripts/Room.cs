using UnityEngine;

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

