using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water: MonoBehaviour, IWeapon
{
    public float Cooldown => 2.0f; // Water 무기의 쿨다운 시간

    public void Punch()
    {
        Debug.Log("Water Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}

