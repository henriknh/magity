using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Player : MonoBehaviour {
    
    Rigidbody2D rb;

    public float speedForward = 6f;
    public float jumpPower = 7f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpTime;

    bool isJumping = false;
    float jumpTimeCounter;

    public AudioClip jumpSound;
    public AudioClip landSound;


    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();

        SetField();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isPaused) {
            rb.Sleep();
            return;
        }
        rb.WakeUp();

        Vector3 velocity = rb.velocity;
        velocity.x = speedForward;
        rb.velocity = velocity;

        if(isJumping && jumpTimeCounter > 0)
        {
            rb.velocity += jumpPower * -Physics2D.gravity.normalized;
            jumpTimeCounter -= Time.fixedDeltaTime;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Physics2D.gravity.normalized * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if(rb.velocity.y > 0 && isJumping)
        {
            //rb.velocity += Physics2D.gravity.normalized * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public void SetJumping(bool jump)
    {
        isJumping = jump;
        if (jump)
        {
            jumpTimeCounter = jumpTime;
            MundoSound.Play(jumpSound);
        }
    }

    void OnBecameInvisible()
    {
        if(!GameManager.instance.gameActive)
            return;

        GameManager.instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // MundoSound.Play(landSound);
    }

    public void SetField()
    {
        string materialPath = materialPath = "Materials/RedPole";

        if (WorldManager.instance.fieldActiveRed)
        {
            materialPath = "Materials/BluePole";
        }

        Material material = Resources.Load(materialPath, typeof(Material)) as Material;
        SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        sprite.material = material;

    }
}