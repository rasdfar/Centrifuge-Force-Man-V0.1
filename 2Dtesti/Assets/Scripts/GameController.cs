using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public string LevelToLoad;

    void Update()
    {

    }


    public void OpenScene()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
