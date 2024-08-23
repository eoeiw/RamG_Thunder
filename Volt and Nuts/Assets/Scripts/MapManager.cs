using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Vector2Int currentRoomPosition = Vector2Int.zero; // 현재 방의 좌표
    public GameObject portalPrefab; // 포탈 프리팹
    public GameObject player; // 플레이어 캐릭터 오브젝트
    public GameObject fadeEffect; // 검은 사각형 스프라이트 오브젝트

    public List<Room> rooms; // 생성된 방 리스트

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

        // 4. 기존 포탈을 제거하고 새로운 포탈을 배치합니다.
        ClearPortals();
        PlacePortals(currentRoomPosition);

        // 5. 현재 방 위치를 콘솔에 출력합니다.
        Debug.Log($"현재 방 위치: {currentRoomPosition}");

        // 6. 페이드 아웃 효과를 시작합니다.
        yield return StartCoroutine(FadeOut());
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

    private void PlacePortals(Vector2Int roomPosition)
    {
        // 방의 인접한 위치에 포탈 배치
        Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        Vector3[] portalPositions = {
            new Vector3(-18f, 1.2f, 0f),
            new Vector3(23f, 1.2f, 0f),
            new Vector3(3f, 18f, 0f),
            new Vector3(3f, -15f, 0f)
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

    private void InstantiatePortal(Vector3 position, Vector2Int targetRoomPosition)
    {
        GameObject portal = Instantiate(portalPrefab, position, Quaternion.identity);
        PortalScript portalScript = portal.GetComponent<PortalScript>();
        portalScript.targetRoomPosition = targetRoomPosition;
        portalScript.mapManager = this; // 포탈에 MapManager를 참조하게 함
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

    private void ClearPortals()
    {
        PortalScript[] existingPortals = FindObjectsOfType<PortalScript>();
        foreach (PortalScript portal in existingPortals)
        {
            Destroy(portal.gameObject);
        }
    }
}
