using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IWeapon
{
    public float Cooldown => 1.5f; // Plant 무기의 쿨다운 시간

    public void Punch()
    {
        Debug.Log("Plant Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}