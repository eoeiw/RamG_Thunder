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


