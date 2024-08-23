using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.5f; // Wind 무기의 쿨다운 시간
    [SerializeField] private GameObject windPrefab; // 바람 효과 프리팹
    [SerializeField] private Transform windSpawnPoint; // 바람 생성 위치
    [SerializeField] private float radius = 1f; // 원의 반지름
    [SerializeField] private float lifetime = 0.5f; // 바람 효과의 생명주기 (초)

    private void Update()
    {
        MouseFollowWithOffset(); // 플레이어가 마우스를 따라 회전하도록 업데이트
    }

    public void Punch()
    {
        // 마우스 위치를 스크린 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(windSpawnPoint.position);

        // 플레이어가 바라보는 방향에 따른 중심점 설정
        Vector3 center = mousePos.x < playerScreenPoint.x ? new Vector3(0, 0.5f, 0) : new Vector3(0, 0.7f, 0);

        // 플레이어의 방향에 따른 원주 위의 점 계산
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;

        Vector3 windOffset = new Vector3(
            radius * Mathf.Cos(angle * Mathf.Deg2Rad),
            center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        );

        // 발사 위치를 오프셋을 적용하여 계산
        Vector3 windPosition = windSpawnPoint.position + windOffset;

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        mouseWorldPosition.z = 0; // 2D 공간에서의 위치로 조정

        // 발사 위치와 마우스 위치 간의 방향 계산
        Vector3 direction = (mouseWorldPosition - windPosition).normalized;
        float windAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // 바람 효과 생성
        GameObject newWind = Instantiate(windPrefab, windPosition, Quaternion.Euler(0, 0, windAngle));

        // 바람 효과를 일정 시간 후에 삭제
        StartCoroutine(DestroyWindAfterDelay(newWind, lifetime));

        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    private IEnumerator DestroyWindAfterDelay(GameObject windObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(windObject);
    }

    private void MouseFollowWithOffset()
    {
        // 마우스 위치를 스크린 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(windSpawnPoint.position);

        // 마우스 위치와 플레이어 위치 간의 각도 계산
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            // 마우스가 플레이어의 왼쪽에 있을 때
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, angle);
        }
        else
        {
            // 마우스가 플레이어의 오른쪽에 있을 때
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
