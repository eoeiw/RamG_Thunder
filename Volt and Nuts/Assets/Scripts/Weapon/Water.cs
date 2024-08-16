using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water: MonoBehaviour, IWeapon
{
    public float Cooldown => 2.0f; // Water ������ ��ٿ� �ð�

    public void Punch()
    {
        Debug.Log("Water Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}

