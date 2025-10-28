using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    const string PLAYER = "Player";

    PlayerDropPlatform playerDrop;
    PlatformEffector2D platformEffector2D;

    private void Awake()
    {
        platformEffector2D = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(PLAYER))
        {
            playerDrop = collision.gameObject.GetComponent<PlayerDropPlatform>();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(playerDrop == null) 
        {
            return;
        }
        if (playerDrop.fallThrough)
        {
            platformEffector2D.rotationalOffset = 180;
            playerDrop = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerDrop = null;
        platformEffector2D.rotationalOffset = 0;
    }
}
