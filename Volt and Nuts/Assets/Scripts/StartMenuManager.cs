using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // Map_Inventor 씬으로 이동
        SceneManager.LoadScene("Map_Inventor");
    }
}
