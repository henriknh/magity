using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class InputManager : MonoBehaviour {

    public static InputManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void RegisterTouchEvents()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
    }

    public void UneregisterTouchEvents()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
        LeanTouch.OnFingerSwipe -= OnFingerSwipe;
    }

    void OnFingerDown(LeanFinger finger)
    {
        if (finger.IsOverGui || GameManager.instance.isPaused)
            return;

        WorldManager.instance.player.GetComponent<Player>().SetJumping(true);
    }

    void OnFingerUp(LeanFinger finger)
    {
        if (finger.IsOverGui || GameManager.instance.isPaused)
            return;

        WorldManager.instance.player.GetComponent<Player>().SetJumping(false);
    }

    void OnFingerSwipe(LeanFinger finger)
    {
        if (finger.IsOverGui || GameManager.instance.isPaused)
            return;

        WorldManager.instance.ChangeFieldDirection();
        Physics2D.gravity = -Physics2D.gravity;
    }
}
