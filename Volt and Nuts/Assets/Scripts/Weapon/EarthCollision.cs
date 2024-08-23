using UnityEngine;

public class EarthCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 트리거 충돌 처리
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth.TakeDamage(4); // 데미지 값을 적절히 설정
        }
    }
}
