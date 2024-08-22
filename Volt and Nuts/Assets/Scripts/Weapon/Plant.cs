using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.5f; // plant 무기의 쿨다운 시간

    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private Transform plantSpawnPoint;
    [SerializeField] private Vector3 plantOffset = new Vector3(0, 1, 0); // 발사 위치 오프셋

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Punch()
    {
        MouseFollowWithOffset(); // 무기의 회전 조정

        // 발사 위치를 오프셋을 적용하여 계산
        Vector3 plantPosition = plantSpawnPoint.position + plantOffset;

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));

        // 발사 위치와 마우스 위치 간의 방향 계산
        Vector3 direction = (mouseWorldPosition - plantPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 투사체 생성 및 발사
        GameObject newplant = Instantiate(plantPrefab, plantPosition, Quaternion.Euler(0, 0, angle));

        ActiveWeapon.Instance.ToggleIsAttacking(false); // 공격 끝. ActiveWeapon의 isAttacking 값을 False로 변환
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

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

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
