using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;

    private bool attackButtonDown = false;
    private bool isAttacking = false;
    private bool attackPerformed = false; // ������ �̹� ����Ǿ����� ���θ� Ȯ���ϴ� ����
    private float cooldownTimer = 0.0f; // ��ٿ� Ÿ�̸�

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        playerControls.Combat.Punch.started += _ => StartPunch();
        playerControls.Combat.Punch.canceled += _ => StopPunch();
    }

    private void Update()
    {
        Punch();
        UpdateCooldown(); // ��ٿ� Ÿ�̸� ������Ʈ
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        // ���Ⱑ ����Ǹ� ��ٿ� Ÿ�̸Ӹ� ������ ��ٿ� �ð����� ����
        cooldownTimer = (CurrentActiveWeapon as IWeapon)?.Cooldown ?? 0.0f;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void StartPunch()
    {
        attackButtonDown = true;
    }

    private void StopPunch()
    {
        attackButtonDown = false;
    }

    private void Punch()
    {
        if (attackButtonDown && !isAttacking && !attackPerformed && cooldownTimer <= 0)
        {
            isAttacking = true;
            attackPerformed = true; // ������ ����Ǿ����� ǥ��
            (CurrentActiveWeapon as IWeapon)?.Punch();
            cooldownTimer = (CurrentActiveWeapon as IWeapon)?.Cooldown ?? 0.0f; // ��ٿ� Ÿ�̸� ����
        }
        else if (!attackButtonDown)
        {
            attackPerformed = false; // ���� ��ư�� ������ ������ ���� ���� ���¸� ����
        }
    }

    private void UpdateCooldown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime; // ��ٿ� Ÿ�̸� ����
        }
    }
}
