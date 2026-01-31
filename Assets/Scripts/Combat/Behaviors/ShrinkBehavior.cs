using UnityEngine;

public class ShrinkBehavior : IBulletBehavior
{
    private float duration;
    private float t;

    public ShrinkBehavior(float duration)
    {
        this.duration = duration;
    }

    public void Tick(Bullet bullet, float dt)
    {
        t += dt;
        float scale = Mathf.Clamp01(1f - t / duration);
        bullet.transform.localScale = Vector3.one * scale;

        if (scale <= 0f)
            bullet.gameObject.SetActive(false);
    }
}