using System.Collections;
using UnityEngine;

public class Wind : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.5f; // Wind ������ ��ٿ� �ð�
    [SerializeField] private GameObject windPrefab; // �ٶ� ȿ�� ������
    [SerializeField] private Transform windSpawnPoint; // �ٶ� ���� ��ġ
    [SerializeField] private float radius = 1f; // ���� ������
    [SerializeField] private float lifetime = 0.5f; // �ٶ� ȿ���� �����ֱ� (��)

    private void Update()
    {
        MouseFollowWithOffset(); // �÷��̾ ���콺�� ���� ȸ���ϵ��� ������Ʈ
    }

    public void Punch()
    {
        // ���콺 ��ġ�� ��ũ�� ��ǥ�� ��ȯ
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(windSpawnPoint.position);

        // �÷��̾ �ٶ󺸴� ���⿡ ���� �߽��� ����
        Vector3 center = mousePos.x < playerScreenPoint.x ? new Vector3(0, 0.5f, 0) : new Vector3(0, 0.7f, 0);

        // �÷��̾��� ���⿡ ���� ���� ���� �� ���
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;

        Vector3 windOffset = new Vector3(
            radius * Mathf.Cos(angle * Mathf.Deg2Rad),
            center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        );

        // �߻� ��ġ�� �������� �����Ͽ� ���
        Vector3 windPosition = windSpawnPoint.position + windOffset;

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        mouseWorldPosition.z = 0; // 2D ���������� ��ġ�� ����

        // �߻� ��ġ�� ���콺 ��ġ ���� ���� ���
        Vector3 direction = (mouseWorldPosition - windPosition).normalized;
        float windAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // �ٶ� ȿ�� ����
        GameObject newWind = Instantiate(windPrefab, windPosition, Quaternion.Euler(0, 0, windAngle));

        // �ٶ� ȿ���� ���� �ð� �Ŀ� ����
        StartCoroutine(DestroyWindAfterDelay(newWind, lifetime));

        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    private IEnumerator DestroyWindAfterDelay(GameObject windObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(windObject);
    }

    private void MouseFollowWithOffset()
    {
        // ���콺 ��ġ�� ��ũ�� ��ǥ�� ��ȯ
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(windSpawnPoint.position);

        // ���콺 ��ġ�� �÷��̾� ��ġ ���� ���� ���
        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            // ���콺�� �÷��̾��� ���ʿ� ���� ��
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, angle);
        }
        else
        {
            // ���콺�� �÷��̾��� �����ʿ� ���� ��
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
