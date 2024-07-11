using UnityEngine;

public enum CharacterState
{
    Idle,
    Moving,
    Dashing,
    Attacking
}
public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 7f;
    public float acceleration = 40f;
    public float deceleration = 40f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.3f; // 대쉬 쿨타임
    public float attackCooldown = 0.2f; // 공격 쿨타임
    public float attackDuration = 0.05f;
    public float attackSpeed = 50f;
    private Vector2 movement;
    private Vector2 dashDirection;
    private Vector2 mouseDirection;
    private CharacterState currentState = CharacterState.Idle;
    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private float stateTimer;
    private float dashCooldownTimer; // 대쉬 쿨타임 타이머
    private float attackCooldownTimer; // 대쉬 쿨타임 타이머

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
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

    void Update()
    {
        dashCooldownTimer -= Time.deltaTime; // 쿨타임 타이머 감소
        attackCooldownTimer -= Time.deltaTime; // 쿨타임 타이머 감소

        mouseDirection = GetMouseDirection();

        switch (currentState)
        {
            case CharacterState.Idle:
                if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0) // 대쉬 키
                {
                    StartDash();
                }
                else if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0) // 공격 키
                {
                    StartAttack();
                }
                break;

            case CharacterState.Moving:
                if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
                {
                    StartDash();
                }
                else if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
                {
                    StartAttack();
                }
                break;

            case CharacterState.Dashing:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    rb.velocity *= 0.2f;
                    currentState = CharacterState.Idle;
                }
                break;

            case CharacterState.Attacking:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    rb.velocity *= 0.1f;
                    currentState = CharacterState.Idle;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == CharacterState.Idle || currentState == CharacterState.Moving)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        if (movement.sqrMagnitude > 0)
        {
            currentState = CharacterState.Moving;
            rb.AddForce(movement.normalized * acceleration);

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            currentState = CharacterState.Idle;
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
    }

    private void StartDash()
    {
        currentState = CharacterState.Dashing;
        stateTimer = dashDuration;
        dashCooldownTimer = dashCooldown; // 대쉬 쿨타임 설정
        dashDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector3.right;
        }
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);
    }

    private void StartAttack()
    {
        currentState = CharacterState.Attacking;
        stateTimer = attackDuration;
        attackCooldownTimer = attackCooldown;
        rb.AddForce(mouseDirection * attackSpeed, ForceMode2D.Impulse);
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - rb.position).normalized;
        return direction;
    }
}
