using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
        public Color color;

    public ColorPlayer()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        color = Color.white;
        this.spriteRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        this.spriteRenderer.color = color;
    }

    public void ChangeColor(Color color)
    {
        this.color = color;
    }
}
