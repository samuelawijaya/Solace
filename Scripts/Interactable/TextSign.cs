using UnityEditor;
using UnityEngine;

public class TextSign : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject uiPanel; // assign your UI panel in Inspector
    private bool isPlayerInRange = false;
    private bool isSignActive = false;
    private GameObject signSelected;
    private GameObject signNormal;

    [SerializeField] AudioClip signOpenSound;

    private void Awake()
    {
        signSelected = transform.Find("Selected").gameObject;
        signNormal = transform.Find("Sign").gameObject;
        signSelected.SetActive(false);
        signNormal.SetActive(true);
    }
    private void Start()
    {
        gameInput.OnInteract += GameInput_OnInteractSign;
    }

    private void GameInput_OnInteractSign(object sender, System.EventArgs e)
    {
        
        if (isSignActive) 
        {
            SoundManager.instance.PlaySoundClip(signOpenSound, transform, 1f);
            uiPanel.SetActive(false); // hide sign
            isSignActive = false;
            Time.timeScale = 1f;     // resume game
        }
        else if (isPlayerInRange && !isSignActive) // Press E to interact
        {
            SoundManager.instance.PlaySoundClip(signOpenSound, transform, 1f);
            if (uiPanel != null)
            {
                isSignActive = true;
                uiPanel.SetActive(true); // show sign
                Time.timeScale = 0f;     // pause game
            }
        }
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            signSelected.SetActive(true);
            signNormal.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            signSelected.SetActive(false);
            signNormal.SetActive(true);
        }
    }
}
