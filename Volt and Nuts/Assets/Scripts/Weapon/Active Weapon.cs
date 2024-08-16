using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;

    private bool attackButtonDown = false;
    private bool isAttacking = false;
    private bool attackPerformed = false; // 공격이 이미 수행되었는지 여부를 확인하는 변수
    private float cooldownTimer = 0.0f; // 쿨다운 타이머

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
        UpdateCooldown(); // 쿨다운 타이머 업데이트
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        // 무기가 변경되면 쿨다운 타이머를 무기의 쿨다운 시간으로 설정
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
            attackPerformed = true; // 공격이 수행되었음을 표시
            (CurrentActiveWeapon as IWeapon)?.Punch();
            cooldownTimer = (CurrentActiveWeapon as IWeapon)?.Cooldown ?? 0.0f; // 쿨다운 타이머 설정
        }
        else if (!attackButtonDown)
        {
            attackPerformed = false; // 공격 버튼이 눌리지 않으면 공격 수행 상태를 리셋
        }
    }

    private void UpdateCooldown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime; // 쿨다운 타이머 감소
        }
    }
}
