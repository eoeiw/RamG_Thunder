using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 다람쥐 스크립트
/// </summary>
public class Bolt : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;

    [SerializeField] private Transform punchCollider; // 07.26 김영훈 : 아마... Punch Collider 라는 오브젝트 찾기용 변수
    private Transform bone3; // 07.19 김영훈 : 자식 오브젝트 중 bone_3 찾기 위해 필요함


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();

        bone3 = transform.Find("Bolt_S/bone_2/bone_3"); // 07.19 김영훈 : 자식 오브젝트 중 bone_3 찾는 과정. 오브젝트 이름 바뀌면 수정해야함.
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Punch.started += _ => StartPunch();
    }

    private void StartPunch()
    {
        myAnimator.SetTrigger("Punch");
        punchCollider.gameObject.SetActive(true);

    }


    public void StopPunch()
    {
        punchCollider.gameObject.SetActive(false);
    }

}
