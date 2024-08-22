using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile4 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Animator myAnimator;

    private void Awake()
    {
        // Animator 컴포넌트 참조
        myAnimator = GetComponent<Animator>();
    }


    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        // Projectile이 현재 방향으로 움직이도록 수정
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}