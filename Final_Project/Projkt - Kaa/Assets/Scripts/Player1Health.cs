﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Health : MonoBehaviour
{
    public Sprite[] sprites;
    public int Health;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Health = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health > 3)
        {
            Health = 3;
        }

        if (Health == 3)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (Health == 2)
        {
            spriteRenderer.sprite = sprites[1];
        }
        else if (Health == 1)
        {
            spriteRenderer.sprite = sprites[2];
        }
        else if (Health == 0)
        {
            spriteRenderer.sprite = sprites[3];
        }

    }
}
