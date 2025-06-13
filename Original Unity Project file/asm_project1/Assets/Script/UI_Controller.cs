using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    private float f_timer = 0;
    public TMP_Text t_timer;
    public TMP_Text t_pauseMenu;
    public TMP_Text t_pauseButton;
    public GameObject pauseMenu;

    public GameObject player;
    private PlayerController playerControlScript;

    public bool gamePause = false;
    public bool gameOver = false;

    private string timeText;

    // Start is called before the first frame update
    void Start()
    {
        t_pauseMenu.text = "";
        playerControlScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePause && !gameOver)
        {
            f_timer += Time.deltaTime;
            float min = Mathf.FloorToInt(f_timer / 60);
            float sec = Mathf.FloorToInt(f_timer % 60);
            float ms = Mathf.FloorToInt((f_timer * 100) % 100);
            timeText = "Time Used " + min.ToString() + ":" + sec.ToString() + ":" + ms.ToString();
            t_timer.text = timeText;
        }
    }

    void OnPause()
    {
        if (!gameOver)
        {
            gamePause = !gamePause;
            if (gamePause)
            {
                t_pauseButton.text = "Resume";
                t_pauseMenu.text = "Paused";
                PauseGame();
            }
            else {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        gamePause = true;
        playerControlScript.gamePause = gamePause;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        
            //+ "<br>" + "Press Esc to resume the game";
    }

    public void ResumeGame()
    {
        gamePause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        playerControlScript.gamePause = gamePause;
        if (gameOver)
        {
            gameOver = false;
            playerControlScript.StartGame();
            f_timer = 0;
            t_timer.text = "Time Used 0:0:0";
        }
        else
        {
            //t_pauseMenu.text = "";
        }
    }

    public void winGame()
    {
        gameOver = true;
        PauseGame();
        t_pauseButton.text = "Restart";
        t_pauseMenu.text = "You Win!" + "<br>" + timeText + "<br>" + "scores: " + playerControlScript.score.ToString() + "/ 17";
    }

    public void loseGame()
    {
        gameOver = true;
        PauseGame();
        t_pauseButton.text = "Restart";
        t_pauseMenu.text = "You Lose!" + "<br>" + timeText + "<br>" + "scores: " + playerControlScript.score.ToString() + "/ 17";
    }
}
