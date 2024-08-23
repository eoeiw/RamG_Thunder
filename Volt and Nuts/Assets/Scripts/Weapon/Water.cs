using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour, IWeapon
{
    public float Cooldown => 2.0f; // Water ������ ��ٿ� �ð�
    [SerializeField] private GameObject shieldPrefab; // ��ȣ�� ������
    [SerializeField] private Transform waterSpawnPoint; // ���� ȿ�� ���� ��ġ
    [SerializeField] private Vector3 shieldOffset = new Vector3(0, 1, 0); // ��ȣ�� ������
    [SerializeField] private float shieldLifetime; // ��ȣ�� �����ֱ�

    private GameObject shieldInstance; // ���� ��ȣ�� �ν��Ͻ�
    private Coroutine shieldCoroutine; // ��ȣ�� �����ֱ� �ڷ�ƾ

    public void Punch()
    {
        Debug.Log("Water Attack");

        // ��ȣ�� ����
        if (shieldPrefab != null && shieldInstance == null)
        {
            // �÷��̾� �������� ��ȣ�� ��ġ ���
            Vector3 shieldPosition = PlayerController.Instance.transform.position + shieldOffset;
            shieldInstance = Instantiate(shieldPrefab, shieldPosition, Quaternion.identity);

            // ��ȣ�� �����ֱ� ����
            if (shieldCoroutine != null)
            {
                StopCoroutine(shieldCoroutine); // ���� �ڷ�ƾ ����
            }
            shieldCoroutine = StartCoroutine(DestroyShieldAfterDelay(shieldLifetime));
        }

        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    private IEnumerator DestroyShieldAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (shieldInstance != null)
        {
            Destroy(shieldInstance);
            shieldInstance = null; // �ν��Ͻ��� null�� �����Ͽ� ���� ����
        }
    }

    public void DestroyShield()
    {
        if (shieldInstance != null)
        {
            Destroy(shieldInstance);
            shieldInstance = null; // �ν��Ͻ��� null�� �����Ͽ� ���� ����
        }
    }

    private void Update()
    {
        // ��ȣ���� ������ ���, ��ġ�� �÷��̾��� ��ġ�� ���߾� ������Ʈ
        if (shieldInstance != null)
        {
            Vector3 shieldPosition = PlayerController.Instance.transform.position + shieldOffset;
            shieldInstance.transform.position = shieldPosition;
        }
    }
}
