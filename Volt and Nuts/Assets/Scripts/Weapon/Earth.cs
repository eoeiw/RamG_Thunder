using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour, IWeapon
{
    public float Cooldown => 1.5f; // Earth 무기의 쿨다운 시간

    public void Punch()
    {
        Debug.Log("Earth Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
