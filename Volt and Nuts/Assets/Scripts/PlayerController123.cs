using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControlle123 : MonoBehaviour
{
    [SerializeField]
    //---------------------------------이동------------------------------//
    public float maxSpeed = 5f; // 최고 속도
    public float acceleration = 40f; // 가속도
    public float deceleration = 40f; // 감속도
    public float dashSpeed = 20f; // 대시 속도
    public float dashDuration = 0.2f; // 대시 지속 시간
    public float dashCooldown = 0.3f; // 대시 쿨타임
    private bool isDashing = false;
    private bool canDash = true;
    private bool dashEnd = false;
    private float dashTime;
    private float nextDashTime;

    //-----------------------------공격------------------------------//
    private float attackCooldown = 0.3f;
    private float attackRange = 2f;
    private float attackVelocity = 2f;
    private float attackCharge = 1.5f;
    private float elementType = 0f; // 원소 타입  ( 0~6까지 나중에 바꾸는 기능 추가 예정)
    private bool canAttack = true;
    private float nextAttackTime;

    //-------------------------------참조-------------------------------//
    private TrailRenderer myTrailRenderer;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Vector2 vision;
    private Rigidbody2D rb;

    private PlayerNormal normal; //공격 (원소 없음)
    private Camera mainCamera;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        normal = GetComponent<PlayerNormal>();
        myTrailRenderer = GetComponent<TrailRenderer>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleInput();
        UpdateElement();
    }

    private void FixedUpdate()
    {
        vision = GetMouseDirection();

        HandleDash();
        HandleAttack();
        HandleMovement();
    }

    private void HandleInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void HandleMovement()
    {
        if (!isDashing)
        {
            Move();
        }

        if (dashEnd)
        {
            rb.velocity *= 0.3f;
            dashEnd = false;
        }
    }

    private void Move()
    {
        if (movement.sqrMagnitude > 0)
        {
            rb.AddForce(movement * acceleration);

            // 최고 속도 제한
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            // 감속
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
    }

    private void HandleDash()
    {
        if (isDashing && Time.time >= dashTime)
        {
            EndDash();
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && canDash && !isDashing)
        {
            StartDash(movement);
        }

        if (!canDash && Time.time >= nextDashTime)
        {
            canDash = true;
        }
    }

    private void StartDash(Vector2 direction)
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        rb.velocity = direction.normalized * dashSpeed;
        myTrailRenderer.emitting = true;
    }

    private void EndDash()
    {
        isDashing = false;
        canDash = false;
        myTrailRenderer.emitting = false;
        dashEnd = true;
        nextDashTime = Time.time + dashCooldown;
    }

    private void HandleAttack()
    {
        if (canAttack && Time.time >= nextAttackTime && Input.GetMouseButtonDown(0))
        {
            Attack(vision);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack(Vector2 attackDirection)
    {
        normal.NormalAttack(attackDirection, attackRange, attackVelocity);
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - rb.position).normalized;
        return direction;
    }

    private void UpdateElement()
    {
        switch (elementType)
        {
            case 0:
                attackCooldown = normal.attackCooldown;
                attackRange = normal.attackRange;
                attackVelocity = normal.attackVelocity;
                attackCharge = normal.attackCharge;
                break;
            // 다른 원소 타입에 대한 처리가 여기에 추가될 예정
        }
    }
}


