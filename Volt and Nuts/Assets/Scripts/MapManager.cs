using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class MapManager : MonoBehaviour
{
    public Vector2Int currentRoomPosition = Vector2Int.zero; // 현재 방의 좌표
    public GameObject portalPrefab; // 포탈 프리팹
    public GameObject player; // 플레이어 캐릭터 오브젝트
    public GameObject fadeEffect; // 검은 사각형 스프라이트 오브젝트
    public GameObject enemyPrefab; // 적 프리팹
    public int numberOfEnemies = 5; // 생성할 적의 수
    private int enemiesRemaining; // 현재 남아 있는 적의 수

    public List<Room> rooms; // 생성된 방 리스트
    private Dictionary<Vector2Int, bool> clearedRooms = new Dictionary<Vector2Int, bool>(); // 방의 클리어 여부를 저장하는 딕셔너리

    private void Start()
    {
        rooms = RandomMapGenerator.instance.rooms; // 맵 데이터를 가져옴
        PlacePortals(currentRoomPosition); // 포탈 배치
    }

    public void MoveToRoom(Vector2Int newRoomPosition)
    {
        StartCoroutine(RoomTransition(newRoomPosition));
    }

    private IEnumerator RoomTransition(Vector2Int newRoomPosition)
    {
        // 1. 페이드 인 효과를 시작합니다.
        yield return StartCoroutine(FadeIn());

        // 2. 플레이어의 위치를 (0,0)으로 초기화합니다.
        player.transform.position = Vector3.zero;

        // 3. 방 좌표를 갱신합니다.
        currentRoomPosition = newRoomPosition;

        // 4. 기존 포탈을 제거합니다.
        ClearPortals();

        Room currentRoom = GetRoomAtPosition(currentRoomPosition);

        // 5. 현재 방이 클리어되지 않았다면 적을 생성합니다.
        if (!IsRoomCleared(currentRoomPosition))
        {
            SpawnEnemies();
        }
        else if (currentRoom.Info == 2)
        {
            // 출구 방이고 적을 모두 처치했으면 Scene2로 이동 (보스맵)
            MoveToBossStage();
        }
        else
        {
            // 다른 클리어된 방이라면 포탈만 생성
            PlacePortals(currentRoomPosition);
        }

        // 6. 현재 방 위치를 콘솔에 출력합니다.
        Debug.Log($"현재 방 위치: {currentRoomPosition}");

        // 7. 페이드 아웃 효과를 시작합니다.
        yield return StartCoroutine(FadeOut());
    }

    private bool IsRoomCleared(Vector2Int roomPosition)
    {
        // 해당 방이 클리어된 적이 있는지 딕셔너리에서 확인합니다.
        return clearedRooms.ContainsKey(roomPosition) && clearedRooms[roomPosition];
    }

    public void OnEnemyKilled()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            Room currentRoom = GetRoomAtPosition(currentRoomPosition);

            if (currentRoom.Info == 2)
            {
                // 출구 방에서 적을 모두 처치하면 보스 스테이지로 이동
                MoveToBossStage();
            }
            else
            {
                // 다른 방에서는 그냥 포탈 생성
                clearedRooms[currentRoomPosition] = true;
                PlacePortals(currentRoomPosition);
            }
        }
    }

    private void MoveToBossStage()
    {
        // 보스 스테이지로 이동
        SceneManager.LoadScene("Scene2");
    }

    private void SpawnEnemies()
    {
        enemiesRemaining = numberOfEnemies;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            // 랜덤한 위치에 적 소환
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // 적이 (0, 0)에서 떨어진 랜덤한 위치에 생성되도록 함
        float x = Random.Range(5f, 10f) * (Random.Range(0, 2) == 0 ? 1 : -1); // 양수나 음수로 랜덤하게 생성
        float y = Random.Range(5f, 10f) * (Random.Range(0, 2) == 0 ? 1 : -1);
        return new Vector3(x, y, 0f);
    }

    private void ClearPortals()
    {
        PortalScript[] existingPortals = FindObjectsOfType<PortalScript>();
        foreach (PortalScript portal in existingPortals)
        {
            Destroy(portal.gameObject);
        }
    }

    private void PlacePortals(Vector2Int roomPosition)
    {
        // 방의 인접한 위치에 포탈 배치
        Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        Vector3[] portalPositions = {
            new Vector3(-35f, 1.2f, 0f),
            new Vector3(38f, 1.2f, 0f),
            new Vector3(0f, 16f, 0f),
            new Vector3(0f, -16f, 0f)
        };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int adjacentPosition = roomPosition + directions[i];
            if (IsRoomAtPosition(adjacentPosition))
            {
                InstantiatePortal(portalPositions[i], adjacentPosition);
            }
        }
    }

    private bool IsRoomAtPosition(Vector2Int position)
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

    private Room GetRoomAtPosition(Vector2Int position)
    {
        foreach (Room room in rooms)
        {
            if (room.Position == position)
            {
                return room;
            }
        }
        return null;
    }

    private void InstantiatePortal(Vector3 position, Vector2Int targetRoomPosition)
    {
        GameObject portal = Instantiate(portalPrefab, position, Quaternion.identity);
        PortalScript portalScript = portal.GetComponent<PortalScript>();
        portalScript.targetRoomPosition = targetRoomPosition;
        portalScript.mapManager = this; // 포탈에 MapManager를 참조하게 함
    }

    private IEnumerator FadeIn()
    {
        SpriteRenderer spriteRenderer = fadeEffect.GetComponent<SpriteRenderer>();
        fadeEffect.SetActive(true);

        // 알파 값을 0에서 1로 천천히 증가시켜서 페이드 인 효과를 줍니다.
        for (float alpha = 0; alpha <= 1; alpha += Time.deltaTime)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        SpriteRenderer spriteRenderer = fadeEffect.GetComponent<SpriteRenderer>();

        // 알파 값을 1에서 0으로 천천히 감소시켜서 페이드 아웃 효과를 줍니다.
        for (float alpha = 1; alpha >= 0; alpha -= Time.deltaTime)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }

        // 페이드 아웃이 끝난 후 오브젝트를 비활성화합니다.
        fadeEffect.SetActive(false);
    }
}
