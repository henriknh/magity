using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public bool gameActive = false;
    public bool gameOver = false;
    public int score = 0;
    public int highscore = 0;
    public bool isPaused = false;

    Coroutine scoreCoroutine;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        StorageManager.instance.Load();
    }

    private void FixedUpdate()
    {
        if (isPaused)
            return;

        if (gameActive) score++;
    }

    public void StartGame()
    {
        gameActive = true;
        gameOver = false;
        score = 0;
        isPaused = false;

        Physics2D.gravity = -(new Vector2(Mathf.Abs(Physics2D.gravity.x), Mathf.Abs(Physics2D.gravity.y)));

        WorldManager.instance.StartWorld();
        InputManager.instance.RegisterTouchEvents();
    }

    public void GameOver(bool quit = false)
    {
        InputManager.instance.UneregisterTouchEvents();

        gameActive = false;
        gameOver = true;

        bool newHighscore = false;
        if(score > highscore)
        {
            highscore = score;
            newHighscore = true;
            StorageManager.instance.Save();
        }

        StartCoroutine(UIManager.instance.DoGameOver(newHighscore, quit));
    }
}
