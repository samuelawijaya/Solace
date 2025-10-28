using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;

    private const string PLAYER = "Player";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        levelLoader.LoadNextLevel();
    }
}
