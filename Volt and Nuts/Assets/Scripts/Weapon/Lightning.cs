using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.2f; // Lightning ������ ��ٿ� �ð�

    public void Punch()
    {
        Debug.Log("Lightning Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
