using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Earth : MonoBehaviour, IWeapon
{
    public float Cooldown => 1f; // Earth 무기의 쿨다운 시간

    [SerializeField] private GameObject earthPrefab; // Earth 프리팹
    [SerializeField] private float delayBeforeDrop = 1f; // 떨어지기 전의 지연 시간
    [SerializeField] private float lifetime = 1f; // Earth가 생성된 후 사라지기 전의 시간

    private Vector3 spawnPosition; // 마우스를 클릭한 위치

    private void Awake()
    {
        // 아무 초기화 작업이 필요하지 않음
    }

    public void Punch()
    {
        // 마우스 위치를 기억하고 지연 후 Earth 생성
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        spawnPosition.z = 0; // 2D 공간에서의 위치로 조정

        StartCoroutine(SpawnEarthWithDelay());
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    private IEnumerator SpawnEarthWithDelay()
    {
        // 지연 시간 후 Earth 생성
        yield return new WaitForSeconds(delayBeforeDrop);

        // 기억된 위치에서 Earth 생성
        GameObject newEarth = Instantiate(earthPrefab, spawnPosition, Quaternion.identity);

        // Earth가 1초 후에 사라지도록 설정
        Destroy(newEarth, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거 충돌 처리
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth?.TakeDamage(2); // 데미지값을 여기서 설정하거나, 개별 변수로 설정 가능
            Destroy(gameObject);
        }
    }
}
