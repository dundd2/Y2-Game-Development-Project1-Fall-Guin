using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void onGameStart()
    {
        SceneManager.LoadScene("Asg1", LoadSceneMode.Single);
        canvas.SetActive(false);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Asg1"));
    }
}
