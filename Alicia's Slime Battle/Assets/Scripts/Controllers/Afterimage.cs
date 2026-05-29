using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/*
 * This class represents the afterimage of entities while they dash
 */
public class Afterimage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // The reference to the afterimage object pool has a public setter and private getter
    private IObjectPool<Afterimage> pool;
    public IObjectPool<Afterimage> Pool { set => pool = value; }

    private float duration;
    public float Duration { set => duration = value; }


    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite, Vector3 scale) {
        spriteRenderer.sprite = sprite;
        transform.localScale = scale;
    }

    public void SetSprite(Sprite sprite, Color color, Vector3 scale) {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        transform.localScale = scale;
    }

    public void StartFade() {
        StartCoroutine(Fade(duration));
    }

    // Afterimage linearly fades away by time within duration
    IEnumerator Fade(float duration) {
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;
        float initialAlpha = color.a;

        while (spriteRenderer.color.a > 0) {
            color.a = Mathf.Lerp(initialAlpha, 0f, elapsedTime / duration);
            spriteRenderer.color = color;
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        pool.Release(this);
    }
}
