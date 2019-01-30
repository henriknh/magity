using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    public static WorldManager instance = null;

    public GameObject playerPrefab;

    [HideInInspector]
    public GameObject player;

    public delegate void ChangeField(bool fieldActive);
    [HideInInspector]
    public static event ChangeField ChangeFieldRed;
    [HideInInspector]
    public static event ChangeField ChangeFieldBlue;
    [HideInInspector]
    public bool fieldActiveRed = false;
    [HideInInspector]
    public bool fieldActiveBlue = true;

    [HideInInspector]
    public bool spawnRedPoles = true;
    [HideInInspector]
    public bool spawnBluePoles = true;

    Coroutine changeBiomeCoroutine;
    Coroutine flashCoroutine;

    public AudioClip changeFieldSound;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartWorld()
    {
        fieldActiveRed = false;
        fieldActiveBlue = true;

        spawnRedPoles = true;
        spawnBluePoles = true;

        PoleManager.instance.InitPoles();

        player = Instantiate(playerPrefab);

        PoleManager.instance.StartSpawnPoles();
        CoinManager.instance.StartSpawnCoins();

        changeBiomeCoroutine = StartCoroutine(DoChangeBiome());
    }

    public void Destory()
    {
        if (changeBiomeCoroutine != null)
            StopCoroutine(changeBiomeCoroutine);
        if(flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        PoleManager.instance.Destroy();
        CoinManager.instance.Destroy();
        CameraManager.instance.GameOver();

        Destroy(player);
    }

    public void ChangeFieldDirection()
    {
        fieldActiveRed = !fieldActiveRed;
        fieldActiveBlue = !fieldActiveBlue;

        ChangeFieldRed?.Invoke(fieldActiveRed);
        ChangeFieldBlue?.Invoke(fieldActiveBlue);

        player.GetComponent<Player>().SetField();

        MundoSound.Play(changeFieldSound);
    }

    IEnumerator DoChangeBiome()
    {
        for (; ; )
        {
            float timeout = 0f;
            while(timeout < 15)
            {
                if (!GameManager.instance.isPaused)
                    timeout += Time.deltaTime;
                yield return null;
            }

            int mode = Random.Range(0, 3);
            switch(mode)
            {
                case 0:
                    spawnRedPoles = true;
                    spawnBluePoles = true;
                    break;
                case 1:
                    spawnRedPoles = true;
                    spawnBluePoles = false;
                    break;
                case 2:
                    spawnRedPoles = false;
                    spawnBluePoles = true;
                    break;
            }

            flashCoroutine = StartCoroutine(CameraManager.instance.DoFlash());
        }
    }
}
