using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IWeapon
{
    public float Cooldown => 1.5f; // Fire 무기의 쿨다운 시간

    public void Punch()
    {
        Debug.Log("Fire Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false); // 공격 끝. ActiveWeapon의 isAttacking 값을 False로 변환
    }
}
