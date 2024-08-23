using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.2f; // Lightning 무기의 쿨다운 시간

    [SerializeField] private GameObject lightningPrefab; // 번개 프리팹
    [SerializeField] private Vector3 lightningOffset = new Vector3(0, 0, 0); // 번개 위치 오프셋
    [SerializeField] private float destroyAfterSeconds = 0.22f; // 번개가 사라지기 전 대기 시간

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Punch()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        // 번개 위치에 오프셋 적용
        Vector3 lightningPosition = mouseWorldPosition + lightningOffset;

        // 번개 생성
        GameObject newLightning = Instantiate(lightningPrefab, lightningPosition, Quaternion.identity);

        // 일정 시간 후에 번개 제거
        Destroy(newLightning, destroyAfterSeconds);

        // 충돌 처리를 위한 컴포넌트 추가
        LightningCollision collisionHandler = newLightning.AddComponent<LightningCollision>();

        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

}
