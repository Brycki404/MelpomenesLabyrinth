using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    BossController bossController;

    private void Awake()
    {
        bossController = GetComponentInParent<BossController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var GO = other.gameObject;
        var bullet = GO.GetComponent<Bullet>();
        if (bullet != null && (bullet.Faction == BulletFaction.Player || bullet.Faction == BulletFaction.Neutral))
        {
            if (gameObject.name.Contains("LeftHand"))
                bossController.DamageLeftHand(1f);
            else if (gameObject.name.Contains("RightHand"))
                bossController.DamageRightHand(1f);
            else if (gameObject.name.Contains("Mask"))
                bossController.DamageBoss(1f);
            else
                return;
            GO.SetActive(false);
        }
    }
}