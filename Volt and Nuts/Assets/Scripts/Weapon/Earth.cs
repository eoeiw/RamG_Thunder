using System.Collections;
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

        // �浹 ó���� ���� ������Ʈ �߰�
        EarthCollision collisionHandler = newEarth.AddComponent<EarthCollision>();

        // Earth�� 1�� �Ŀ� ��������� ����
        Destroy(newEarth, lifetime);
    }
}
