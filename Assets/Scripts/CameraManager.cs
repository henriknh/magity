using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance = null;

    Coroutine scoreCoroutine;

    float cameraSpeed = 0f;

    public Material background;
    public Material redMaterial;
    public Material blueMaterial;

    Color targetBackgroundColor;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        targetBackgroundColor = background.color;

        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        if (GameManager.instance.isPaused)
            return;

        if (GameManager.instance.gameActive)
        {
            GameObject player = GameObject.FindWithTag("Player");
            var pos = Vector3.zero;
            pos.x = player.transform.position.x + 4f;
            Camera.main.transform.position = pos;
            cameraSpeed = player.GetComponent<Player>().speedForward;
        } else
        {
            Vector3 newPos = Camera.main.transform.position;
            newPos.x += cameraSpeed * Time.fixedDeltaTime;
            Camera.main.transform.position = newPos;
        }

        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, targetBackgroundColor, 25f * Time.deltaTime);
    }

    public void GameOver()
    {
        targetBackgroundColor = background.color;
    }

    public IEnumerator DoFlash()
    {
        float h, s, v;
        Color wantedColor;

        if(WorldManager.instance.spawnRedPoles)
            wantedColor = redMaterial.color;
        else
            wantedColor = blueMaterial.color;

        Color.RGBToHSV(wantedColor, out h, out s, out v);
        s *= 0.2f;
        targetBackgroundColor = Color.HSVToRGB(h, s, v);
        yield return new WaitUntil(() => Camera.main.backgroundColor == targetBackgroundColor);
        yield return new WaitForSeconds(0.05f);
        targetBackgroundColor = background.color;
        yield return new WaitUntil(() => Camera.main.backgroundColor == targetBackgroundColor);

        if (WorldManager.instance.spawnBluePoles)
            wantedColor = blueMaterial.color;
        else
            wantedColor = redMaterial.color;

        Color.RGBToHSV(wantedColor, out h, out s, out v);
        s *= 0.2f;
        targetBackgroundColor = Color.HSVToRGB(h, s, v);
        yield return new WaitUntil(() => Camera.main.backgroundColor == targetBackgroundColor);
        yield return new WaitForSeconds(0.05f);
        targetBackgroundColor = background.color;
        yield return new WaitUntil(() => Camera.main.backgroundColor == targetBackgroundColor);
    }
}
