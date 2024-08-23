using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ٶ��� ��ũ��Ʈ
/// </summary>
public class Bolt : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;

    [SerializeField] private Transform punchCollider; // 07.26 �迵�� : �Ƹ�... Punch Collider ��� ������Ʈ ã��� ����
    private Transform bone3; // 07.19 �迵�� : �ڽ� ������Ʈ �� bone_3 ã�� ���� �ʿ���


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();

        bone3 = transform.Find("Bolt_S/bone_2/bone_3"); // 07.19 �迵�� : �ڽ� ������Ʈ �� bone_3 ã�� ����. ������Ʈ �̸� �ٲ�� �����ؾ���.
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
