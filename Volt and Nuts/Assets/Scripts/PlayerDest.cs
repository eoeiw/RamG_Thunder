using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCleanup : MonoBehaviour
{
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Player 태그를 가진 오브젝트를 찾아서 삭제
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player); // 씬이 로드될 때 Player 오브젝트를 삭제
        }
    }

    private void OnEnable()
    {
        // 씬 로드 이벤트에 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트에서 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
