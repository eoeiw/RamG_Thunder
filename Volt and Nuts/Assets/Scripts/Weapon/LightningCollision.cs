using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� Health ��ũ��Ʈ�� ã�� �������� ����
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth.TakeDamage(1); // ������ ���� ������ ����
        }
    }
}
