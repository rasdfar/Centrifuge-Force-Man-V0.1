using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public int LevelToLoad;
    public GameObject Player;

    void Start()
    {

    }
    void FixedUpdate()
    {
        //if there is no player object-> respawn
        if (!Player.active)
        {
            OpenScene();
        }
    }


    public void OpenScene()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
