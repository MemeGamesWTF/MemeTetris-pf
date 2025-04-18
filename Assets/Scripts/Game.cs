using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
public class Game : MonoBehaviour
{  
    [SerializeField] GameObject gamePanel, gameoverPanel, pausePanel, htpPanel, pauseButton, score, level, lines;
    [SerializeField] GameObject gameOverTitle, pauseTitle;
    [SerializeField] Tweening tween;
    [SerializeField] Board board;
    [SerializeField] ScoreBoard scoreboard;
    [SerializeField] AudioManager audioManager;

     [DllImport("__Internal")]
  private static extern void SendScore(int score, int game);

    public bool isPaused = false;

    private void Start()
    {
        audioManager.PlaySound("main menu");
    }

    internal void GameOver()
    {
        audioManager.StopSound("theme");
        audioManager.PlaySound("game over");
        
        // Show Game Over screen
        gameoverPanel.SetActive(true);
        pauseButton.SetActive(false);
        tween.PulsatingTitle(gameOverTitle);
        SendScore(scoreboard.score, 105);
    }

    public void PlayButton()
    {
        audioManager.StopSound("main menu");
        audioManager.PlaySound("theme");
        gamePanel.SetActive(false);
        pauseButton.SetActive(true);
        score.SetActive(true);
        level.SetActive(true);
        lines.SetActive(true);
        board.BeginGame();
    }

    public void PauseButton()
    {
        audioManager.PauseSound("theme");
        audioManager.PlaySound("pause");
        isPaused = true;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        tween.PulsatingTitle(pauseTitle);
    }

    public void ResumeButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        audioManager.PlaySound("theme");
        isPaused = false;
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void PlayAgainButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        isPaused = false;
        audioManager.PlaySound("theme");
        scoreboard.RestartValues();
        gameoverPanel.SetActive(false);
        pauseButton.SetActive(true);
        board.BeginGame();
    }

 
    public void HtpButton ()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (htpPanel.activeSelf)
        {
            htpPanel.SetActive(false);
        } else
        {
            htpPanel.SetActive(true);
        }
    }

    public void MainMenuButton()
    {
        audioManager.StopSound("theme");
        audioManager.PlaySound("main menu");
        isPaused = false;
        board.CleanBoard();
        scoreboard.RestartValues();
        gameoverPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void QuitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }

    public void MoveLeft()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("FallingTetromino");
        gameObject.GetComponent<Tetromino>().MoveLeft();
    }

    public void MoveRight()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("FallingTetromino");
        gameObject.GetComponent<Tetromino>().MoveRight();
    }

    public void Rotate(){
        GameObject gameObject = GameObject.FindGameObjectWithTag("FallingTetromino");
        gameObject.GetComponent<Tetromino>().Rotate();
    }
    public void SoftDrop(){
        GameObject gameObject = GameObject.FindGameObjectWithTag("FallingTetromino");
        gameObject.GetComponent<Tetromino>().SoftDrop();
    }
    public void HardDrop(){
        GameObject gameObject = GameObject.FindGameObjectWithTag("FallingTetromino");
        gameObject.GetComponent<Tetromino>().HardDrop();
    }
}
