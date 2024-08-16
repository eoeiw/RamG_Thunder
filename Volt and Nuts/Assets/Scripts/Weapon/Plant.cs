using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IWeapon
{
    public float Cooldown => 1.5f; // Plant ������ ��ٿ� �ð�

    public void Punch()
    {
        Debug.Log("Plant Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}