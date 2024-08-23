using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float projectileRange; // Projectile�� ��Ÿ�

    private Animator myAnimator;
    private Vector3 startPosition;

    private void Awake()
    {
        // Animator ������Ʈ ����
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
            enemyHealth?.TakeDamage(2); // ���������� ���⼭ �����ϰų�, ���� ������ ���� ����
            Destroy(gameObject);
        }
    }

    private void DetectDistance()
    {
        // ��Ÿ��� �ʰ��ϸ� ����ü �ı�
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        // Projectile�� ���� �������� �����̵��� ����
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
