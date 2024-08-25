using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;

    private int currentHealth;
    private MapManager mapManager;

    private void Start()
    {
        currentHealth = startingHealth;
        mapManager = FindObjectOfType<MapManager>(); // MapManager 찾기
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            mapManager.OnEnemyKilled(); // 적이 죽었을 때 MapManager에 알림
            Destroy(gameObject); // 적 오브젝트 파괴
        }
    }
}


