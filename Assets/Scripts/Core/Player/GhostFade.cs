using UnityEngine;

public class GhostFade : MonoBehaviour
{
    public float lifetime = 0.3f;
    public float fadeSpeed = 5f;

    SpriteRenderer sr;
    Color startColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;

        // Fade out
        Color c = sr.color;
        c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * fadeSpeed);
        sr.color = c;

        if (lifetime <= 0f)
            Destroy(gameObject);
    }
}