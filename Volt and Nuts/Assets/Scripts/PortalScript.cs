using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public Vector2Int targetRoomPosition; // 이동할 방의 좌표
    public MapManager mapManager; // MapManager 참조

    private bool playerIsInTrigger = false; // 플레이어가 포탈 안에 있는지 확인하는 변수

    private void OnTriggerEnter2D(Collider2D other) // 2D용 트리거 이벤트
    {
        Debug.Log($"충돌한 오브젝트: {other.gameObject.name}"); // 충돌한 오브젝트 이름 출력
        if (other.CompareTag("Player"))
        {
            playerIsInTrigger = true; // 플레이어가 포탈에 들어옴
            Debug.Log("플레이어가 포탈에 닿았습니다.");
        }
    }

    private void OnTriggerExit2D(Collider2D other) // 2D용 트리거 이벤트
    {
        Debug.Log($"충돌을 벗어난 오브젝트: {other.gameObject.name}"); // 충돌을 벗어난 오브젝트 이름 출력
        if (other.CompareTag("Player"))
        {
            playerIsInTrigger = false; // 플레이어가 포탈에서 벗어남
        }
    }

    private void Update()
    {
        Debug.Log($"playerIsInTrigger 상태: {playerIsInTrigger}"); // 현재 상태 출력
        if (playerIsInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F 키가 눌렸습니다. 방 이동을 시도합니다.");
            mapManager.MoveToRoom(targetRoomPosition);
        }
    }
}


