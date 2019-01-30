using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    GameObject titleScreen;
    GameObject highscoreScreen;
    GameObject gameScreen;

    Text score;
    Text scoreHighscore;
    GameObject titlePlay;
    GameObject titleHighscore;
    Text curTitleText;
    Image transition;


    GameObject gamePause;
    GameObject gameQuit;
    GameObject gameResume;
    GameObject gameResumeText;

    bool muted = false;
    float targetAlpha = 0f;
    float fadeRate = 3f;
    float targetFlashAlpha = 0f;
    float titleCycleRate = 0.33f;
    float targetAlphaTitle = 0f;

    Coroutine cycleTitleTextCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        titleScreen = GameObject.Find("Title");
        highscoreScreen = GameObject.Find("Highscore");
        gameScreen = GameObject.Find("Game");

        score = GameObject.Find("Score_Game").GetComponent<Text>();
        scoreHighscore = GameObject.Find("Score_Highscore").GetComponent<Text>();
        titlePlay = GameObject.Find("Text_Play");
        titleHighscore = GameObject.Find("Text_Highscore");
        transition = GameObject.Find("Transition").GetComponent<Image>();

        gamePause = GameObject.Find("Button_Pause");
        gameQuit = GameObject.Find("Button_Quit");
        gameResume = GameObject.Find("Button_Resume");
        gameResumeText = GameObject.Find("Text_Resume");

        titleScreen.SetActive(true);
        highscoreScreen.SetActive(false);
        gameScreen.SetActive(false);

        cycleTitleTextCoroutine = StartCoroutine(DoCycleTitleText());
    }

    private void OnGUI()
    {
        if(score != null)
        {
            score.text = GameManager.instance.score.ToString();
        }

        Color curColor = transition.color;
        float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
        if (alphaDiff > 0.0001f)
        {
            curColor.a = Mathf.Lerp(transition.color.a, targetAlpha, fadeRate * Time.deltaTime);
            transition.color = curColor;
        }

        curColor = curTitleText.color;
        alphaDiff = Mathf.Abs(curColor.a - targetAlphaTitle);
        if (alphaDiff > 0.0001f)
        {
            if (curColor.a > targetAlphaTitle)
            {
                curColor.a -= titleCycleRate * Time.deltaTime;
            }
            if (curColor.a < targetAlphaTitle)
            {
                curColor.a += titleCycleRate * Time.deltaTime;
            }
            curTitleText.color = curColor;
        }
    }

    public void OnClickPlay()
    {

        if (GameManager.instance.gameActive) return;

        StartCoroutine(DoStartGame());
    }

    public void OnClickIAP()
    {
        Debug.Log("IAP");
    }

    public void OnClickSound()
    {
        if (muted)
        {
            Debug.Log("Unmute");
        }
        else
        {
            Debug.Log("Mute");
        }
        muted = !muted;
    }

    public void OnClickPause()
    {
        bool isPaused = !GameManager.instance.isPaused;

        gamePause.SetActive(!isPaused);
        gameQuit.SetActive(isPaused);
        gameResume.SetActive(isPaused);
        gameResumeText.SetActive(isPaused);

        GameManager.instance.isPaused = isPaused;
    }

    public void OnClickQuit()
    {
        GameManager.instance.GameOver(true);
    }

    IEnumerator DoStartGame()
    {
        this.targetAlpha = 1f;
        yield return new WaitUntil(() => Mathf.Abs(transition.color.a - targetAlpha) <= 0.01f);

        StopCoroutine(cycleTitleTextCoroutine);

        GameManager.instance.StartGame();

        titleScreen.SetActive(false);
        highscoreScreen.SetActive(false);
        gameScreen.SetActive(true);

        gamePause.SetActive(true);
        gameQuit.SetActive(false);
        gameResume.SetActive(false);
        gameResumeText.SetActive(false);

        this.targetAlpha = 0f;
        yield return new WaitUntil(() => Mathf.Abs(transition.color.a - targetAlpha) <= 0.01f);
    }

    public IEnumerator DoGameOver(bool newHighscore, bool quit)
    {
        if(!quit)
            yield return new WaitForSeconds(1f);

        this.targetAlpha = 1f;
        yield return new WaitUntil(() => Mathf.Abs(transition.color.a - targetAlpha) <= 0.01f);
        
        // Destroy world
        WorldManager.instance.Destory();

        // Handle new highscore
        if (newHighscore)
        {
            scoreHighscore.text = GameManager.instance.highscore.ToString();

            titleScreen.SetActive(false);
            highscoreScreen.SetActive(true);
            gameScreen.SetActive(false);

            this.targetAlpha = 0f;
            yield return new WaitUntil(() => Mathf.Abs(transition.color.a - targetAlpha) <= 0.01f);

            yield return new WaitForSeconds(1f);

            this.targetAlpha = 1f;
            yield return new WaitUntil(() => Mathf.Abs(transition.color.a - targetAlpha) <= 0.01f);
        }

        titleScreen.SetActive(true);
        highscoreScreen.SetActive(false);
        gameScreen.SetActive(false);

        this.targetAlpha = 0f;
        yield return new WaitUntil(() => Mathf.Abs(transition.color.a - targetAlpha) <= 0.01f);

        cycleTitleTextCoroutine = StartCoroutine(DoCycleTitleText());
    }

    IEnumerator DoCycleTitleText()
    {
        Color titlePlayColor = titlePlay.GetComponent<Text>().color;
        titlePlayColor.a = 0;
        titlePlay.GetComponent<Text>().color = titlePlayColor;
        Color titleHighscoreColor = titleHighscore.GetComponent<Text>().color;
        titleHighscoreColor.a = 0;
        titleHighscore.GetComponent<Text>().color = titleHighscoreColor;

        titleHighscore.GetComponent<Text>().text = "HIGHSCORE\n" + GameManager.instance.highscore.ToString();

        for (; ; )
        {

            titlePlay.SetActive(true);
            titleHighscore.SetActive(false);

            if (titlePlay.activeSelf)
                curTitleText = titlePlay.GetComponent<Text>();
            else
                curTitleText = titleHighscore.GetComponent<Text>();
            
            targetAlphaTitle = 1f;
            yield return new WaitUntil(() => Mathf.Abs(curTitleText.color.a - targetAlphaTitle) <= 0.01f);
            yield return new WaitForSeconds(0.77f);
            targetAlphaTitle = 0f;
            yield return new WaitUntil(() => Mathf.Abs(curTitleText.color.a - targetAlphaTitle) <= 0.01f);
            yield return new WaitForSeconds(0.5f);

            titlePlay.SetActive(false);
            titleHighscore.SetActive(true);

            if (titlePlay.activeSelf)
                curTitleText = titlePlay.GetComponent<Text>();
            else
                curTitleText = titleHighscore.GetComponent<Text>();

            targetAlphaTitle = 1f;
            yield return new WaitUntil(() => Mathf.Abs(curTitleText.color.a - targetAlphaTitle) <= 0.01f);
            yield return new WaitForSeconds(0.77f);
            targetAlphaTitle = 0f;
            yield return new WaitUntil(() => Mathf.Abs(curTitleText.color.a - targetAlphaTitle) <= 0.01f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
