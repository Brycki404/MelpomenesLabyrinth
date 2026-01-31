using UnityEngine;

public class FadeBehavior : IBulletBehavior
{
    private float duration;
    private float t;
    private SpriteRenderer sr;

    public FadeBehavior(SpriteRenderer sr, float duration)
    {
        this.sr = sr;
        this.duration = duration;
    }

    public void Tick(Bullet bullet, float dt)
    {
        t += dt;
        float alpha = Mathf.Clamp01(1f - t / duration);

        var c = sr.color;
        c.a = alpha;
        sr.color = c;

        if (alpha <= 0f)
            bullet.gameObject.SetActive(false);
    }
}