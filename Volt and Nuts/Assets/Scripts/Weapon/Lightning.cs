using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.2f; // Lightning ������ ��ٿ� �ð�

    [SerializeField] private GameObject lightningPrefab; // ���� ������
    [SerializeField] private Vector3 lightningOffset = new Vector3(0, 0, 0); // ���� ��ġ ������
    [SerializeField] private float destroyAfterSeconds = 0.22f; // ������ ������� �� ��� �ð�

    private Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Punch()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        // ���� ��ġ�� ������ ����
        Vector3 lightningPosition = mouseWorldPosition + lightningOffset;

        // ���� ����
        GameObject newLightning = Instantiate(lightningPrefab, lightningPosition, Quaternion.identity);

        // ���� �ð� �Ŀ� ���� ����
        Destroy(newLightning, destroyAfterSeconds);

        // �浹 ó���� ���� ������Ʈ �߰�
        LightningCollision collisionHandler = newLightning.AddComponent<LightningCollision>();

        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

}
