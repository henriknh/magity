using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleManager : MonoBehaviour {
    public static PoleManager instance = null;

    public GameObject polePrefab;
    public float spawnPoleWhenDistance = 20f;

    Coroutine spawnPoleCoroutine;

    float lastRedPolePos;
    float lastBluePolePos;

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
        StopCoroutine(spawnPoleCoroutine);

        GameObject[] poles = GameObject.FindGameObjectsWithTag("Pole");

        foreach (GameObject pole in poles)
        {
            Destroy(pole);
        }
    }

    public void InitPoles()
    {
        GameObject poleRed = Instantiate(polePrefab);
        GameObject poleBlue = Instantiate(polePrefab);

        Pole poleRedScript = poleRed.GetComponent<Pole>();
        Pole poleBlueScript = poleBlue.GetComponent<Pole>();

        poleRedScript.Init("Red");
        poleBlueScript.Init("Blue");

        poleRedScript.SetPosition(-10f);
        poleBlueScript.SetPosition(-10f);

        poleRedScript.SetScale(25f);
        poleBlueScript.SetScale(25f);

        poleRedScript.heightFieldDisabled = poleRedScript.heightFieldActive;
        poleBlueScript.heightFieldDisabled = poleBlueScript.heightFieldActive;
        poleRedScript.UpdateField(true);
        poleBlueScript.UpdateField(true);

        lastRedPolePos = poleRed.transform.position.x + poleRed.transform.localScale.x;
        lastBluePolePos = poleBlue.transform.position.x + poleBlue.transform.localScale.x;
    }

    void SpawnPole()
    {
        Vector3 playerPos = WorldManager.instance.player.transform.position;

        if (playerPos.x + spawnPoleWhenDistance > lastRedPolePos && WorldManager.instance.spawnRedPoles)
        {
            int distance = Random.Range(2, 5);
            int size = Random.Range(4, 8);
            GameObject pole = Instantiate(polePrefab);
            Pole poleScript = pole.GetComponent<Pole>();
            poleScript.Init("Red");
            poleScript.SetPosition(lastRedPolePos + distance);
            poleScript.SetScale(size);
            lastRedPolePos = pole.transform.position.x + pole.transform.localScale.x;
        }
        else if (!WorldManager.instance.spawnRedPoles)
            lastRedPolePos = playerPos.x + spawnPoleWhenDistance;

        if (playerPos.x + spawnPoleWhenDistance > lastBluePolePos && WorldManager.instance.spawnBluePoles)
        {
            int distance = Random.Range(2, 5);
            int size = Random.Range(4, 8);
            GameObject pole = Instantiate(polePrefab);
            Pole poleScript = pole.GetComponent<Pole>();
            poleScript.Init("Blue");
            poleScript.SetPosition(lastBluePolePos + distance);
            poleScript.SetScale(size);
            lastBluePolePos = pole.transform.position.x + pole.transform.localScale.x;
        }
        else if (!WorldManager.instance.spawnBluePoles)
            lastBluePolePos = playerPos.x + spawnPoleWhenDistance;
    }

    public void StartSpawnPoles()
    {
        spawnPoleCoroutine = StartCoroutine(DoSpawnPoles());
    }

    IEnumerator DoSpawnPoles()
    {
        for (; ; )
        {
            SpawnPole();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
