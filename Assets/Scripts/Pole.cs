using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Pole : MonoBehaviour
{
    public float expandSpeed = 10f;
    public float heightFieldActive = 5f;
    public float heightFieldDisabled = 1f;
    public GameObject poleBox;

    string poleType;
    bool consumed = false;
    float targetHeight;

    public void Init(string poleType)
    {
        this.poleType = poleType;
        targetHeight = heightFieldDisabled;
        Material material = Resources.Load("Materials/" + poleType + "Pole", typeof(Material)) as Material;
        SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        sprite.material = material;
        
        Vector3 pos = transform.position;

        if (poleType == "Red")
        {
            pos.y = Random.Range(2, 5);

            Vector3 scale = poleBox.transform.localScale;
            scale.y = -1;
            poleBox.transform.localScale = scale;
            poleBox.transform.position = new Vector3(0.5f, 0.5f, 0f);
            sprite.flipY = true;

            if (WorldManager.instance.fieldActiveRed)
                targetHeight = heightFieldActive;
            WorldManager.ChangeFieldRed += UpdateField;
        }
        if (poleType == "Blue")
        {
            pos.y = -Random.Range(2, 5);

            if (WorldManager.instance.fieldActiveBlue)
                targetHeight = heightFieldActive;
            WorldManager.ChangeFieldBlue += UpdateField;
        }
        transform.position = pos;
    }

    private void FixedUpdate()
    {
        Vector3 curScale = transform.localScale;
        float heightDiff = targetHeight - curScale.y;
        curScale.y += heightDiff * expandSpeed * Time.fixedDeltaTime;
        transform.localScale = curScale;
    }

    public void UpdateField(bool fieldActive)
    {
        if(fieldActive)
        {
            targetHeight = heightFieldActive;
        } else
        {
            targetHeight = heightFieldDisabled;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        if (poleType == "Red")
        {
            WorldManager.ChangeFieldRed -= UpdateField;
        }
        if (poleType == "Blue")
        {
            WorldManager.ChangeFieldBlue -= UpdateField;
        }
    }

    public void SetPosition(float posValue)
    {
        Vector3 pos = transform.position;
        pos.x = posValue;
        transform.position = pos;
    }

    public void SetScale(float scaleValue)
    {
        Vector3 scale = transform.localScale;
        scale.x = scaleValue;
        transform.localScale = scale;
    }
}