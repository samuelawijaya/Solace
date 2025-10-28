using UnityEngine;
using System.Collections;

public class RoomShadow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer blackOverlay; // Set to the black sprite covering the room
    [SerializeField] private GameObject enemiesInRoom;    // Assign enemy parent here in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOut(0.35f));

            if (enemiesInRoom != null)
            {
                enemiesInRoom.SetActive(true);
            }
        }
    }

    IEnumerator FadeOut(float duration)
    {
        float elapsed = 0f;
        Color startColor = blackOverlay.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            blackOverlay.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        blackOverlay.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        blackOverlay.gameObject.SetActive(false); // optional: disable once invisible
    }
}
