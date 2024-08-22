using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile4 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Animator myAnimator;

    private void Awake()
    {
        // Animator ������Ʈ ����
        myAnimator = GetComponent<Animator>();
    }


    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        // Projectile�� ���� �������� �����̵��� ����
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}