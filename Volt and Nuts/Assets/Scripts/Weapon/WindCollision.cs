using UnityEngine;

public class WindCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� Health ��ũ��Ʈ�� ã�� �������� ����
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth.TakeDamage(3); // ������ ���� ������ ����
        }
    }
}
