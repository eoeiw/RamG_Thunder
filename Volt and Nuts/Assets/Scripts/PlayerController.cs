using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
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

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;

    /////////////// 07.18 김영훈 볼트 애니메이션용 변수

    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;


    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>(); // 07.18 김영훈
        mySpriteRender = GetComponent<SpriteRenderer>(); // 07.18 김영훈
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }


    private void Update()
    {
        PlayerInput();

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && canDash && !isDashing)
        {
            Dash(movement);
        }

        if (!canDash && Time.time >= nextDashTime)
        {
            canDash = true;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing && Time.time >= dashTime)
        {
            isDashing = false;
            canDash = false;
            dashEnd = true;
            nextDashTime = Time.time + dashCooldown;
        }

        if (dashEnd)
        {
            rb.velocity *= 0.3f;
            dashEnd = false;
        }

        if (!isDashing)
        {
            Move();
        }

        AdjustPlayerFacingDirection(); // 07.18 김영훈
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x); // 07.18 김영훈
        myAnimator.SetFloat("moveY", movement.y); // 07.18 김영훈
    }

    private void Move()
    {
        if (movement.x != 0 || movement.y != 0)
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

    private void Dash(Vector2 direction)
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        rb.velocity = direction.normalized * dashSpeed;
    }
    
    private void AdjustPlayerFacingDirection() // 마우스 따라가기 함수 07.18 김영훈
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
        }
        else
        {
            mySpriteRender.flipX = false;
        }
    }
}

