using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.2f; // Fire ������ ��ٿ� �ð�

    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Transform fireSpawnPoint;

    [SerializeField] private Vector3 fireOffset = new Vector3(0, 1, 0); // �߻� ��ġ ������

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Punch()
    {
        MouseFollowWithOffset(); // ������ ȸ�� ����

        // �߻� ��ġ�� �������� �����Ͽ� ���
        Vector3 firePosition = fireSpawnPoint.position + fireOffset;
        
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
       
        // �߻� ��ġ�� ���콺 ��ġ ���� ���� ���
        Vector3 direction = (mouseWorldPosition - firePosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // ����ü ���� �� �߻�
        GameObject newFire = Instantiate(firePrefab, firePosition, Quaternion.Euler(0, 0, angle));

        ActiveWeapon.Instance.ToggleIsAttacking(false); // ���� ��. ActiveWeapon�� isAttacking ���� False�� ��ȯ
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

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


    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
