using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject Room;
    public GameObject PlayerStart;
    public GameObject Next;
    public GameObject Treasure;
    public int gridSize = 5;
    public int minRectangles = 7;
    public int maxRectangles = 10;


    private List<Vector2Int> occupiedPositions = new List<Vector2Int>();
    private int rectangleCount;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        rectangleCount = Random.Range(minRectangles, maxRectangles + 1);

        for (int i = 0; i < rectangleCount; i++)
        {
            PlaceRectangle(i);
        }
    }

    void PlaceRectangle(int RoomNum)
    {
        Vector2Int position = GetRandomPosition();

        while (IsOccupied(position) || !IsAdjacent(position))
        {
            position = GetRandomPosition();
        }

        occupiedPositions.Add(position);
        if(RoomNum == 1)
        {
            Instantiate(PlayerStart, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
        else if(RoomNum == 7)
        {
            Instantiate(Treasure, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
        else if(RoomNum == rectangleCount-1)
        {
            Instantiate(Next, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(Room, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
    }

    Vector2Int GetRandomPosition()
    {
        return new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
    }

    bool IsOccupied(Vector2Int position)
    {
        return occupiedPositions.Contains(position);
    }

    bool IsAdjacent(Vector2Int position)
    {
        if (occupiedPositions.Count == 0) return true;

        foreach (Vector2Int occupiedPosition in occupiedPositions)
        {
            if (Vector2Int.Distance(occupiedPosition, position) == 1)
            {
                return true;
            }
        }

        return false;
    }
}

