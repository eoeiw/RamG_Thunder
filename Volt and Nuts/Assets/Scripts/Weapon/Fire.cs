using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IWeapon
{
    public float Cooldown => 1.5f; // Fire ������ ��ٿ� �ð�

    public void Punch()
    {
        Debug.Log("Fire Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false); // ���� ��. ActiveWeapon�� isAttacking ���� False�� ��ȯ
    }
}
