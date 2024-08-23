using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour, IWeapon
{
    public float Cooldown => 2.0f; // Water 무기의 쿨다운 시간
    [SerializeField] private GameObject shieldPrefab; // 보호막 프리팹
    [SerializeField] private Transform waterSpawnPoint; // 물리 효과 생성 위치
    [SerializeField] private Vector3 shieldOffset = new Vector3(0, 1, 0); // 보호막 오프셋
    [SerializeField] private float shieldLifetime; // 보호막 생명주기

    private GameObject shieldInstance; // 현재 보호막 인스턴스
    private Coroutine shieldCoroutine; // 보호막 생명주기 코루틴

    public void Punch()
    {
        Debug.Log("Water Attack");

        // 보호막 생성
        if (shieldPrefab != null && shieldInstance == null)
        {
            // 플레이어 기준으로 보호막 위치 계산
            Vector3 shieldPosition = PlayerController.Instance.transform.position + shieldOffset;
            shieldInstance = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity);

            // 보호막 생명주기 설정
            if (shieldCoroutine != null)
            {
                StopCoroutine(shieldCoroutine); // 이전 코루틴 중지
            }
            shieldCoroutine = StartCoroutine(DestroyShieldAfterDelay(shieldLifetime));
        }

        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    private IEnumerator DestroyShieldAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (shieldInstance != null)
        {
            Destroy(shieldInstance);
            shieldInstance = null; // 인스턴스를 null로 설정하여 재사용 방지
        }
    }

    public void DestroyShield()
    {
        if (shieldInstance != null)
        {
            Destroy(shieldInstance);
            shieldInstance = null; // 인스턴스를 null로 설정하여 재사용 방지
        }
    }

    private void Update()
    {
        // 보호막이 생성된 경우, 위치를 플레이어의 위치에 맞추어 업데이트
        if (shieldInstance != null)
        {
            Vector3 shieldPosition = PlayerController.Instance.transform.position + shieldOffset;
            shieldInstance.transform.position = shieldPosition;
        }
    }
}
