using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile4 : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float projectileRange; // Projectile의 사거리
    private float timeSinceLaunch = 0f; // 투사체가 발사된 후 경과된 시간을 추적하는 변수
    [SerializeField] private float lifetime; // 투사체가 사라지기 전까지의 최대 생존 시간


    private Animator myAnimator;
    private Vector3 startPosition;


    private void Awake()
    {
        // Animator 컴포넌트 참조
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectDistance();
        CheckLifetime(); // 발사 후 경과 시간을 확인
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth?.TakeDamage(3); // 데미지값을 여기서 설정하거나, 개별 변수로 설정 가능
            Destroy(gameObject);
        }
    }

    private void DetectDistance()
    {
        // 사거리를 초과하면 투사체 파괴
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        // Projectile이 현재 방향으로 움직이도록 수정
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void CheckLifetime()
    {
        // 시간 경과를 체크하고, 시간이 지나면 투사체를 파괴
        timeSinceLaunch += Time.deltaTime;

        if (timeSinceLaunch >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}