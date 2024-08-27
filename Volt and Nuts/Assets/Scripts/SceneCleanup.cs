using UnityEngine;
using UnityEngine.SceneManagement;

public class MapInventorCleanup : MonoBehaviour
{
    private void OnEnable()
    {
        // 씬이 로드되기 전에 이벤트 등록
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        // 씬이 로드되기 전에 이벤트 해제
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        // 씬이 전환될 때 Map_Inventor 태그가 있는 오브젝트 삭제
        GameObject mapInventor = GameObject.FindWithTag("MapInventor");
        if (mapInventor != null)
        {
            Destroy(mapInventor);
            Debug.Log("Map_Inventor 오브젝트 삭제됨");
        }
    }
}

