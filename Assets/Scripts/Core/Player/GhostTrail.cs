using UnityEngine;
using System.Collections;

public class GhostTrail : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 0.05f;
    public float ghostLifetime = 0.3f;
    public Color ghostColor = new Color(1f, 1f, 1f, 0.6f);

    Transform visualTransform;
    SpriteRenderer playerSR;
    bool spawning = false;

    void Awake()
    {
        visualTransform = gameObject.transform.Find("Visual");
        playerSR = visualTransform.GetComponent<SpriteRenderer>();
    }

    public void StartGhostTrail(float duration)
    {
        if (!spawning)
            StartCoroutine(SpawnGhosts(duration));
    }

    public void StopGhostTrail()
    {
        if (spawning == true)
            StopCoroutine("SpawnGhosts");
    }

    IEnumerator SpawnGhosts(float duration)
    {
        spawning = true;

        float t = 0f;
        while (t < duration && spawning == true)
        {
            t += spawnInterval;

            var ghostsParent = GameObject.Find("Ghosts").transform;

            // Spawn ghost
            GameObject g = Instantiate(ghostPrefab, transform.position, transform.rotation);
            if (ghostsParent == null)
            {
                Debug.LogError("Ghosts object NOT found as a direct child of root");
            }
            else
            {
                g.transform.SetParent(ghostsParent, true);
            }
            SpriteRenderer gsr = g.GetComponentInChildren<SpriteRenderer>();

            gsr.sprite = playerSR.sprite;
            gsr.flipX = playerSR.flipX;
            gsr.color = ghostColor;

            // Pass lifetime to ghost
            GhostFade fade = g.GetComponentInChildren<GhostFade>();
            fade.lifetime = ghostLifetime;

            yield return new WaitForSeconds(spawnInterval);
        }

        spawning = false;
    }
}