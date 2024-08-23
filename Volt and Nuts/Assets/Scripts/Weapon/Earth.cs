using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Earth : MonoBehaviour, IWeapon
{
    public float Cooldown => 1f; // Earth ������ ��ٿ� �ð�

    [SerializeField] private GameObject earthPrefab; // Earth ������
    [SerializeField] private float delayBeforeDrop = 1f; // �������� ���� ���� �ð�
    [SerializeField] private float lifetime = 1f; // Earth�� ������ �� ������� ���� �ð�

    private Vector3 spawnPosition; // ���콺�� Ŭ���� ��ġ

    private void Awake()
    {
        // �ƹ� �ʱ�ȭ �۾��� �ʿ����� ����
    }

    public void Punch()
    {
        // ���콺 ��ġ�� ����ϰ� ���� �� Earth ����
        spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        spawnPosition.z = 0; // 2D ���������� ��ġ�� ����

        StartCoroutine(SpawnEarthWithDelay());
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    private IEnumerator SpawnEarthWithDelay()
    {
        // ���� �ð� �� Earth ����
        yield return new WaitForSeconds(delayBeforeDrop);

        // ���� ��ġ���� Earth ����
        GameObject newEarth = Instantiate(earthPrefab, spawnPosition, Quaternion.identity);

        // Earth�� 1�� �Ŀ� ��������� ����
        Destroy(newEarth, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ʈ���� �浹 ó��
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (!other.isTrigger && enemyHealth)
        {
            enemyHealth?.TakeDamage(2); // ���������� ���⼭ �����ϰų�, ���� ������ ���� ����
            Destroy(gameObject);
        }
    }
}
