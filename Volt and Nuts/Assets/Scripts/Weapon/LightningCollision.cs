using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적의 Health 스크립트를 찾고 데미지를 입힘
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth.TakeDamage(1); // 데미지 값을 적절히 설정
        }
    }
}
