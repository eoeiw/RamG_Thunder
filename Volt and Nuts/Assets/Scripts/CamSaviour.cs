using UnityEngine;
using Cinemachine;

public class CamSaviour : MonoBehaviour
{
    private static CamSaviour instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // 이미 존재하는 경우 파괴
        }
    }
}

