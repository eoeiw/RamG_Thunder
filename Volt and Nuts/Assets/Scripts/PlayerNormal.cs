using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerNormal : MonoBehaviour
{
    public float attackCooldown = 0.2f;
    public float attackRange = 10f;
    public float attackVelocity = 20f;
    public float attackCharge = 1.5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    public IEnumerator NormalAttack(Vector2 direction, float Range, float Accel)
    {
        float startTime = Time.time;

        while (Time.time < startTime + Range / Accel)
        {
            rb.velocity = direction * Accel;
            yield return null;
        }

        rb.velocity = Vector2.zero;
    }
}
