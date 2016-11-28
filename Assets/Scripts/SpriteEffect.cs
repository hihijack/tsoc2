using UnityEngine;
using System.Collections;

public class SpriteEffect : MonoBehaviour {

    public SpriteRenderer spriteRender;
    public float durTime;

    public void SetColor(Color color) 
    {
        spriteRender.color = color;
    }
}
