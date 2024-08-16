using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour, IWeapon
{
    public float Cooldown => 0.2f; // Lightning 무기의 쿨다운 시간

    public void Punch()
    {
        Debug.Log("Lightning Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
