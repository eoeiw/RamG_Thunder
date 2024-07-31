using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 다람쥐 행동 관련 스크립트
/// </summary>

public class PlayerController : Singleton<PlayerController>
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
    
    private Transform bone3; // 07.19 김영훈 : 자식 오브젝트 중 bone_3 찾기 위해 필요함
    [SerializeField] private Transform punchCollider; // 07.26 김영훈 : 아마... Punch Collider 라는 오브젝트 찾기용 변수

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>(); // 07.18 김영훈 : Animator 컴포넌트 호출
        mySpriteRender = GetComponent<SpriteRenderer>(); // 07.18 김영훈 : SpriteRenderer 컴포넌트 호출

        bone3 = transform.Find("Bolt_S/bone_2/bone_3"); // 07.19 김영훈 : 자식 오브젝트 중 bone_3 찾는 과정. 오브젝트 이름 바뀌면 수정해야함.

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

        if (bone3 != null) // 07.19 김영훈 : Awake 함수에서 bone3 값을 찾았다면 실행
        {
            //Quaternion : 부드러운 회전 제공
            Quaternion animationRotation = bone3.rotation;
            Quaternion scriptRotation = CalculateScriptRotation();

            bone3.rotation = Quaternion.Lerp(animationRotation, scriptRotation, 0.5f); // 애니메이션과 스크립트의 회전 값 동기화
        }

        /////////////// 07.19 김영훈
        AnimatorStateInfo stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0); // 애니메이션 추가할 때마다 if 추가해야해요..

        if (stateInfo.IsName("Idle")) // Idle 애니메이션 상태일 때
        {
            MouseFollowWithOffset();
        }

        if (stateInfo.IsName("punch")) // punch 애니메이션 상태일 때
        {
            MouseFollowWithOffset();
        }

        if (stateInfo.IsName("walk")) // walk 애니메이션 상태일 때
        {
            MouseFollowWithOffset();
        }

        if (stateInfo.IsName("fastwalk")) // fastwalk 애니메이션 상태일 때
        {
            MouseFollowWithOffset();
        }
    }

    private Quaternion CalculateScriptRotation() // 07.19 김영훈
    {
        // 마우스 위치에 따라 스크립트에서 회전 계산
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = mousePos - playerScreenPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, angle); //근데 이거 사실 MouseFollowWithOffset() 함수랑 똑같은데 어케 동기화를 못하겠어요;; 70 어디에 더하지
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

    private void MouseFollowWithOffset() // 마우스 방향 바라보기
    {

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 bone3ScreenPoint = Camera.main.WorldToScreenPoint(bone3.position); // bone3로 기준점 변경
        Vector3 direction = mousePos - bone3ScreenPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90; // 뼈 기준값이 10.344인가 그래서 임시방편으로 70 더했어요

        // 좌우 반전 적용을 위한 스케일 조정
        if (mousePos.x < playerScreenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // 다람쥐 전체 스케일 좌우반전
            bone3.localScale = new Vector3(1f, 1f, -1f); // 뼈 스케일 반전
            angle = angle + 10; // 값이 좀 달라서 미세조정 해야할 듯 함

        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // 다람쥐 기본 스케일
            bone3.localScale = new Vector3(1f, 1f, 1f); // 뼈 기본 스케일
        }

        // 회전 적용
        bone3.rotation = Quaternion.Euler(0, 0, angle);
        punchCollider.rotation = Quaternion.Euler(0, 0, angle);
    }

}

