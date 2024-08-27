using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoader : MonoBehaviour
{
    void Start()
    {
        Debug.Log("MapInventor 씬 로드됨");
        SceneManager.LoadScene("Scene1");
    }
}

