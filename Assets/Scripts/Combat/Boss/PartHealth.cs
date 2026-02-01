using UnityEngine;
using System;

public class PartHealth : MonoBehaviour
{
    public int maxHP = 50;
    int hp;

    public event Action<PartHealth> OnPartDeath;

    void Start()
    {
        hp = maxHP;
    }

    public void Damage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            hp = 0;
            if (!gameObject.name.Contains("Head"))
                OnPartDeath?.Invoke(this);
            enabled = false;
        }
    }

    public int CurrentHP => hp;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var GO = other.gameObject;
        var bullet = GO.GetComponent<Bullet>();
        if ((bullet != null && (bullet.Faction == BulletFaction.Player || bullet.Faction == BulletFaction.Neutral)) || other.CompareTag("PlayerAttack"))
            Damage(1);
    }
}