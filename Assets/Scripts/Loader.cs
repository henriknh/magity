using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject storageManager;
    public GameObject gameManager;
    public GameObject uiManager;
    public GameObject worldManager;
    public GameObject poleManager;
    public GameObject coinManager;
    public GameObject inputManager;

    void Awake ()
    {
        if (StorageManager.instance == null)
            Instantiate(storageManager);
        if (GameManager.instance == null)
            Instantiate(gameManager);
        if (UIManager.instance == null)
            Instantiate(uiManager);
        if (WorldManager.instance == null)
            Instantiate(worldManager);
        if (PoleManager.instance == null)
            Instantiate(poleManager);
        if (CoinManager.instance == null)
            Instantiate(coinManager);
        if (InputManager.instance == null)
            Instantiate(inputManager);
    }
}
