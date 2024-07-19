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

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Punch.started += _ => Punch();
    }

    private void Punch()
    {
        myAnimator.SetTrigger("Punch");
    }

}


