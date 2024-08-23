using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float projectileRange; // Projectile의 사거리

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
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth?.TakeDamage(2); // 데미지값을 여기서 설정하거나, 개별 변수로 설정 가능
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
}
