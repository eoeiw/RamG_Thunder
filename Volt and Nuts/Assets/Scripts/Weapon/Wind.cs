using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.5f; // Wind 무기의 쿨다운 시간

    public void Punch()
    {
        Debug.Log("Wind Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
