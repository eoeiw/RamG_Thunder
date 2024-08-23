using UnityEngine;

public class EarthCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ʈ���� �浹 ó��
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth.TakeDamage(4); // ������ ���� ������ ����
        }
    }
}
