using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour {
    public static CoinManager instance = null;

    List<Coin> coins = new List<Coin>();

    Coroutine spawnCoinCoroutine;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void Destroy()
    {
        foreach (Coin coin in coins)
        {
            Destroy(coin);
        }
        coins.Clear();

        StopCoroutine(spawnCoinCoroutine);
    }

    void SpawnCoin()
    {

    }

    public void StartSpawnCoins()
    {
        spawnCoinCoroutine = StartCoroutine(DoSpawnCoins());
    }

    IEnumerator DoSpawnCoins()
    {
        for (; ; )
        {
            SpawnCoin();
            yield return new WaitForSeconds(2f);
        }
    }
}
